using System;
using System.Collections.Generic;
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

    public enum StatusEffectType
    {
        None,
        Stun,
        Sleep,
        Burn,
        Poison,
        Bleed,
        Slow,
        Rush
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

        [Tooltip("If effectType == Status, which status to apply")]
        public StatusEffectType statusEffect;

        [Tooltip("Duration in turns (only for status effects)")]
        public int durationInTurns;
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
        public void Cast(Character caster, List<Character> target)
        {
            if (castBehavior != null)
                castBehavior.Cast(caster, target);
            else
                DefaultCast(caster, target);
        }

        // your built-in “basic” logic based on skillType, targetType, effects…
        private void DefaultCast(Character caster, List<Character> targets)
        {
            // check each effect 1 by 1 and apply their effects.
            // prioritize Heal > Damage > Status
            foreach (var effect in effects)
            {
                switch (effect.effectType)
                {
                    case EffectType.Heal:
                        targets.ForEach(t => t.ApplyHeal(effect.magnitude));
                        break;
                    case EffectType.Damage:
                        targets.ForEach(t => t.ApplyDamage(effect.magnitude));
                        break;
                    case EffectType.Status:
                        targets.ForEach(t => t.ApplyStatus(effect.statusEffect, effect.durationInTurns));
                        break;
                }
            }
        }
    }

}