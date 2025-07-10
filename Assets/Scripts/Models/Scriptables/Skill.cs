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
        [Header("Effect Core")]
        public EffectType effectType;

        [Tooltip("For Damage/Heal: amount; for Status: ignored")]
        public int magnitude;

        [Tooltip("If effectType == Status, which status to apply")]
        public StatusEffectType statusEffect;

        [Tooltip("Duration in turns (only for status effects)")]
        public int durationInTurns;
    }
    
    // this class acts as a base class to encapsulate skill behaviors and can not be instantiated directly.
    //[CreateAssetMenu(fileName = "NewSkillBehavior", menuName = "Skill/SkillBehavior")]
    public abstract class Skill : ScriptableObject
    {
        [Header("Identity")]
        public string skillName = "New Skill";
        [TextArea] public string description;
        public Sprite skillIcon;

        [Header("Cost & Cooldown")]
        public int manaCost = 10;
        public int cooldown = 1;

        [Header("Classification")]
        public SkillType skillType;
        public SkillTarget targetType;
        
        [Header("Effects")]
        [Tooltip("You can add multiple effects: Damage + Status, Heal + Buff, etc.")]
        public List<SkillEffect> effects = new List<SkillEffect>();

        [Header("VFX / SFX References")]
        public GameObject vfxPrefab;
        public AudioClip sfxClip;
        
        public abstract void Cast(Character caster, Character target);
    }

}