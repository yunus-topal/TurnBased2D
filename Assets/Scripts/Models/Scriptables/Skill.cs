using Models.Combatants;
using UnityEngine;
using UnityEngine.Serialization;

namespace Models.Scriptables {
    [System.Serializable]
    public struct Skill
    {
        public SkillDataSO data;           // serialized reference to the data asset
        public SkillBehaviorSO behavior;   // serialized reference to the behavior asset

        public Skill(SkillDataSO data, SkillBehaviorSO behavior)
        {
            this.data = data;
            this.behavior = behavior;
        }
    }

    [CreateAssetMenu(fileName = "NewSkillData", menuName = "Combat/SkillData")]
    public class SkillDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        public string SkillName = "New Skill";
        [TextArea] public string description;
        public Sprite SkillIcon;

        [Header("Cost & Cooldown")]
        public int ManaCost = 10;
        public float Cooldown = 1f;

        [Header("Effect Parameters")]
        public float BaseDamage = 50f;
        public float Range = 10f; // most likely irrelevant since it will be a 2d game at the beginning. can be useful later.
        public SkillType SkillType;
        public SkillTarget SkillTarget;

        [Header("VFX / SFX References")]
        public GameObject VfxPrefab;
        public AudioClip SfxClip;
    }

    [CreateAssetMenu(fileName = "NewSkillBehavior", menuName = "Combat/SkillBehavior")]
    public abstract class SkillBehaviorSO : ScriptableObject
    {
        /// <summary>
        /// Called at runtime whenever a character tries to use a skill.
        /// </summary>
        /// <param name="data">The SkillDataSO containing values (damage, VFX, etc.).</param>
        /// <param name="caster">The Character who is casting.</param>
        /// <param name="target">The Character being targeted (can be null if this is an AOE or self-buff, etc.).</param>
        public abstract void Execute(SkillDataSO data, Character caster, Character target);
    }
    public enum SkillType {
        Active,
        Passive,
        Toggle
    }
    
    public enum SkillTarget{
        Self,
        Enemy,
        Ally,
        AllEnemies,
        AllAllies
    }
}