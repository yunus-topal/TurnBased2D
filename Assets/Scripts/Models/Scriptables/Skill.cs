using UnityEngine;

namespace Models.Scriptables {

    // this class acts as a base class to encapsulate skill behaviors and can not be instantiated directly.
    //[CreateAssetMenu(fileName = "NewSkillBehavior", menuName = "Skill/SkillBehavior")]
    public abstract class Skill : ScriptableObject
    {
        [Header("Basic Info")]
        public string SkillName = "New Skill";
        [TextArea] public string description;
        public Sprite SkillIcon;

        [Header("Cost & Cooldown")]
        public int ManaCost = 10;
        public int Cooldown = 1;

        [Header("Effect Parameters")]
        public float BaseDamage = 50f;
        
        public SkillType SkillType;
        public SkillTarget SkillTarget;

        [Header("VFX / SFX References")]
        public GameObject VfxPrefab;
        public AudioClip SfxClip;

        /// <summary>
        /// Called at runtime whenever a character tries to use a skill.
        /// </summary>
        /// <param name="data">The SkillDataSO containing values (damage, VFX, etc.).</param>
        /// <param name="caster">The Character who is casting.</param>
        /// <param name="target">The Character being targeted (can be null if this is an AOE or self-buff, etc.).</param>
        public abstract void Cast(Character caster, Character target);
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