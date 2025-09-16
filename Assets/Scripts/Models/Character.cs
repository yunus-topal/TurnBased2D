using System;
using System.Collections.Generic;
using Helpers;
using Models.Scriptables;
using UnityEngine;

namespace Models {
    public enum Team {Player, Enemy, Neutral}
    public class Character
    {
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

        public void ApplyStatus(StatusEffectType statusEffectType, int durationInTurns)
        {
            
        } 
        
        public override string ToString()
        {
            return $"Character: {Name}" +
                   $"Team: {Team.ToString()}";
        }
        
        // Equality by InstanceId
        public bool Equals(Character other) => other is not null && InstanceId == other.InstanceId;
        public override bool Equals(object obj) => obj is Character c && Equals(c);
        public override int GetHashCode() => InstanceId.GetHashCode();
    }
}
