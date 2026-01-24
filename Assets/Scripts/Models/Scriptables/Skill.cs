using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Models.Scriptables {

    public enum SkillType
    {
        Active,
        Passive
    }

    public enum SkillTarget
    {
        Self,
        SingleEnemy,
        SingleAlly,
        AllEnemies,
        AllAllies
    }

    public enum EffectType
    {
        Damage,
        Heal,
        Status
    }
    
    [Serializable]
    public class SkillEffect
    {
        [Header("Target of this specific effect.")]
        public SkillTarget targetType;
        
        [Header("Effect Core")]
        public EffectType effectType;

        [Tooltip("For Damage/Heal: amount; for Status: ignored")]
        public int magnitude;
        
        [Tooltip("For Damage/Heal: upgraded amount; for Status: ignored")]
        public int magnitudeUpgraded;

        [Tooltip("If effectType == Status, which status to apply")]
        public StatusEffectType statusEffect;

        [Tooltip("Duration in turns (only for status effects)")]
        public int durationInTurns;
        
        [Tooltip("Updated duration in turns (only for status effects)")]
        public int durationInTurnsUpgraded;
        
        [Tooltip("For status effects such as poison/bleed")]
        public int stackCount; 
        
        [Tooltip("For status effects such as poison/bleed")]
        public int updatedStackCount; 
    }
    
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Skill/SkillBehavior")]
    public class Skill : ScriptableObject, ICastable
    {
        [Header("Identity")]
        public string skillName = "New Skill";
        [TextArea] public string description;
        public Sprite skillIcon;
        [Tooltip("Active or Passive")]
        public SkillType skillType = SkillType.Active;

        [Header("Cost & Cooldown")]
        public int manaCost = 10;
        public int cooldown = 1;
        public int upgradedCooldown = 1;

        [Header("Effects")] 
        [Tooltip("Target to cast this skill.")]
        public SkillTarget castingTarget; 
        [Tooltip("You can add multiple effects: Damage + Status, Heal + Buff, etc.")]
        public List<SkillEffect> effects = new List<SkillEffect>();

        [Header("VFX / SFX References")]
        public GameObject vfxPrefab;
        public AudioClip sfxClip;

        [Header("Casting Logic")]
        public CastBehavior castBehavior; 
        public void Cast(Character caster, Character target, List<Character> playerChars, List<Character> enemyChars)
        {
            if (castBehavior != null)
                castBehavior.Cast(caster, target, playerChars, enemyChars);
            else
                DefaultCast(caster, target, playerChars, enemyChars);
        }
        [CanBeNull] private static Character ResolveById(IEnumerable<Character> list, Character c)
            => c == null ? null : list.FirstOrDefault(x => x.InstanceId == c.InstanceId);
        private void DefaultCast(Character caster, Character target, List<Character> playerChars, List<Character> enemyChars)
        {
            bool skillUpgraded = caster.IsSkillUpgraded(this);
            bool casterOnPlayers = playerChars.Any(p => p.InstanceId == caster.InstanceId);
            var allies  = casterOnPlayers ? playerChars : enemyChars;
            var enemies = casterOnPlayers ? enemyChars  : playerChars;
                
            // Snap to canonical instances
            caster = ResolveById(allies, caster) ?? ResolveById(enemies, caster) ?? caster;
            target = ResolveById(allies, target) ?? ResolveById(enemies, target) ?? target;
            
            //Debug.Log("here");
            // go over each effect of the skill and apply them to corresponding targets.
            foreach (var effect in effects)
            {
                List<Character> targets = new();

                // in theory, 1 skill can heal self and deal damage to all allies
                switch (effect.targetType)
                {
                    case SkillTarget.Self:
                        targets.Add(caster);
                        break;
                    case SkillTarget.SingleAlly:
                        { var t = ResolveById(allies, target); if (t != null) targets.Add(t); }
                        break;
                    case SkillTarget.SingleEnemy:
                        { var t = ResolveById(enemies, target); if (t != null) targets.Add(t); }
                        break;
                    case SkillTarget.AllEnemies:
                        targets.AddRange(enemies);
                        break;
                    case SkillTarget.AllAllies:
                        targets.AddRange(allies);
                        break;
                }
                
                var uniqueTargets = targets.GroupBy(c => c.InstanceId).Select(g => g.First());
                
                switch (effect.effectType)
                {
                    case EffectType.Heal:
                        foreach (var t in uniqueTargets) t.ApplyHeal(skillUpgraded ? effect.magnitudeUpgraded : effect.magnitude);
                        break;
                    case EffectType.Damage:
                        foreach (var t in uniqueTargets) t.ApplyDamage(skillUpgraded ? effect.magnitudeUpgraded : effect.magnitude);
                        break;
                    case EffectType.Status:
                        foreach (var t in uniqueTargets) t.ApplyStatus(effect.statusEffect, skillUpgraded ? effect.durationInTurnsUpgraded : effect.durationInTurns);
                        break;
                }
            }
        }

        public (string, string) GetSkillDetails(bool upgraded = false)
        {
            string prefix = upgraded ? "+" : "";  
            var skillNameText = $"{skillName}{prefix}";
            // use upgraded magnitude for upgraded skills
            
            // go over each skill effect, check {{i}} placeholder in description to write actual values.
            //var skillDescText = $"{description}\nTarget: {castingTarget}\n";
            var skillDescText = description;

            for (int i = 0; i < effects.Count; i++)
            {
                // replace {{i}} with correct magnitude
                int value;

                if (effects[i].effectType == EffectType.Status)
                {
                    value = upgraded ? effects[i].durationInTurnsUpgraded : effects[i].durationInTurns;
                }
                else
                {
                    value = upgraded ? effects[i].magnitudeUpgraded : effects[i].magnitude;
                }

                skillDescText = skillDescText.Replace("{{" + i + "}}", value.ToString());
            }
            
            return (skillNameText, skillDescText);
        }
    }

}