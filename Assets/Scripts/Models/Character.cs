using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using JetBrains.Annotations;
using Models.Scriptables;
using UnityEngine;

namespace Models {
    public enum Team {Player, Enemy, Neutral}
    public class Character
    {
        #region Fields
        
        private static int _nextInstanceId = 1; // reset per play session (OK for combat)
        public int InstanceId { get; }          // runtime-unique
        
        private const string characterResourcePath = "Combatants/";
        // character info
        public string Name { get; set; } 
        public int Level { get; set; }
        public int XP { get; set; }
        public Sprite Sprite { get; set; }
        public int MaxHealth { get; set; } 
        public int CurrentHealth { get; set; } 
        public CombatStats CombatStats { get; set; } 
        // only for combat 
        public Team Team { get; set; }
        // equipment and skills and potentially new features.
        public List<Equipment> Equipments { get; set; }
        public List<Skill> Skills { get; set; }
        
        public bool[] skillsUpgraded { get; set; }
        
        public string scriptableObjectPath { get; set; }

        #endregion

        #region Constructors
        public Character(CharacterSO characterScriptable, Team? teamOverride = null)
        {
            InstanceId = _nextInstanceId++;
            
            Name = characterScriptable.characterName;
            Level = characterScriptable.level;
            Sprite = characterScriptable.characterSprite;
            
            // Important: clone mutable data, don’t share the SO’s lists directly.
            CombatStats = characterScriptable.combatStats.Clone();
            Equipments = new List<Equipment>(characterScriptable.equipments);
            Skills = new List<Skill>(characterScriptable.skills);
            Team = teamOverride ?? characterScriptable.team;
            
            // Calculate health based on combat stats.
            MaxHealth = CalculateHealth(CombatStats);
            CurrentHealth = MaxHealth; // Initialize current health to max health.
            scriptableObjectPath = characterResourcePath + characterScriptable.name;
            
            skillsUpgraded = new bool[characterScriptable.skills.Count];
        }
        public Character() { InstanceId = _nextInstanceId++; }
        #endregion

        #region InGameHelpers

        private int CalculateHealth(CombatStats combatStats)
        {
            // TODO: Implement a proper health calculation based on combat stats.
            return 100;
        }

        public void ApplyHeal(int heal)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth = Math.Min(MaxHealth, CurrentHealth + heal);
            }
        }
        
        public void ApplyDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public bool IsSkillUpgraded(Skill skill)
        {
            var index = Skills.IndexOf(skill);
            if (index == -1) return false;
            return skillsUpgraded.ElementAtOrDefault(index);
        }

        #endregion

        #region OtherHelpers
        public override string ToString()
        {
            return $"Character: {Name}" +
                   $"Team: {Team.ToString()}";
        }
        
        // Equality by InstanceId
        public bool Equals(Character other) => other is not null && InstanceId == other.InstanceId;
        public override bool Equals(object obj) => obj is Character c && Equals(c);
        public override int GetHashCode() => InstanceId.GetHashCode();
        
        #endregion

        #region StatusEffectHelpers
        // Inside Models.Character
        private readonly Dictionary<StatusEffectType, StatusInstance> _effects = new();
        private static readonly List<StatusEffectType> _scratchToRemove = new(8); // avoid allocations

        public IReadOnlyCollection<StatusInstance> ActiveEffects => _effects.Values;

        // Apply or stack an effect
        public void ApplyStatus(StatusEffectType type, int durationInTurns, int potencyPerStack = 1, int stacks = 1)
        {
            var rule = StatusLibrary.Rules[type];

            if (_effects.TryGetValue(type, out var existing))
            {
                if (rule.StackMode == StackMode.Additive)
                {
                    existing.Stacks += Math.Max(1, stacks);
                    existing.Duration = Math.Max(existing.Duration, durationInTurns); // keep the longer duration
                    existing.Potency = potencyPerStack; // up to you: overwrite or keep max/avg
                }
                else // non-stacking (e.g., Stun)
                {
                    // don’t stack; refresh/extend duration instead
                    existing.Duration = Math.Max(existing.Duration, durationInTurns);
                    existing.Potency = potencyPerStack; // optional
                }
            }
            else
            {
                var inst = new StatusInstance(type, stacks: rule.StackMode == StackMode.Additive ? Math.Max(1, stacks) : 1,
                                              duration: durationInTurns, potency: potencyPerStack);
                _effects[type] = inst;
            }
        }

        // Convenience checks
        public bool HasBlockingEffect()
        {
            foreach (var kv in _effects)
                if (StatusLibrary.Rules[kv.Key].BlocksTurn) return true;
            return false;
        }

        public StatusEffectType GetBlockingEffect()
        {
            foreach (var kv in _effects)
                if (StatusLibrary.Rules[kv.Key].BlocksTurn) return kv.Key;
            return StatusEffectType.None;
        }

        // Called at the very start of the character’s turn.
        // Returns false if the character is prevented from acting.
        public bool TickTurnStartAndCheckCanAct(Action<string>? debugLog = null)
        {
            // Trigger effects that tick on turn start (e.g., Poison, Sleep side-effects).
            foreach (var kv in _effects.Values)
            {
                var rule = StatusLibrary.Rules[kv.Type];
                if ((rule.TicksWhen & TickTiming.OnTurnStart) != 0)
                    ExecuteEffectTick(kv, isTurnStart:true, debugLog);
            }

            // After start ticks, check whether blocked (Stun/Sleep)
            return !HasBlockingEffect();
        }

        // Called at the very end of the character’s turn (after acting or being skipped)
        public void TickTurnEnd(Action<string>? debugLog = null)
        {
            foreach (var kv in _effects.Values)
            {
                var rule = StatusLibrary.Rules[kv.Type];
                if ((rule.TicksWhen & TickTiming.OnTurnEnd) != 0)
                    ExecuteEffectTick(kv, isTurnStart:false, debugLog);
            }

            // Decay & cleanup
            _scratchToRemove.Clear();
            foreach (var kv in _effects)
            {
                var inst = kv.Value;
                if (StatusLibrary.Rules[kv.Key].DecaysEachTurn)
                    inst.Duration = Math.Max(0, inst.Duration - 1);

                if (inst.Duration <= 0 || CurrentHealth <= 0)
                    _scratchToRemove.Add(kv.Key);
            }
            for (int i = 0; i < _scratchToRemove.Count; i++) _effects.Remove(_scratchToRemove[i]);
        }

        // Centralized per-effect tick behavior
        private void ExecuteEffectTick(StatusInstance inst, bool isTurnStart, Action<string>? debugLog)
        {
            switch (inst.Type)
            {
                case StatusEffectType.Poison:
                    // Damage at start of the afflicted character’s turn
                    // Example: potency per stack; feel free to tune (flat or % MaxHealth)
                    var poisonDmg = inst.Potency * inst.Stacks;
                    ApplyDamage(poisonDmg);
                    debugLog?.Invoke($"{Name} takes {poisonDmg} poison damage ({inst.Stacks} stacks).");
                    break;

                case StatusEffectType.Bleed:
                    // Damage at end of the afflicted character’s turn
                    var bleedDmg = inst.Potency * inst.Stacks;
                    ApplyDamage(bleedDmg);
                    debugLog?.Invoke($"{Name} bleeds for {bleedDmg}.");
                    break;

                case StatusEffectType.Stun:
                    // No damage; blocking handled elsewhere. You might wake early if hit, etc.
                    if (isTurnStart) debugLog?.Invoke($"{Name} is stunned and cannot act.");
                    break;

                case StatusEffectType.Sleep:
                    // Optional: wake on damage; otherwise just block action
                    if (isTurnStart) debugLog?.Invoke($"{Name} is sleeping and skips the turn.");
                    break;

                // add more effects here (Burn, Regen, Vulnerable, etc.)
            }
        }

        

        #endregion
    }
}
