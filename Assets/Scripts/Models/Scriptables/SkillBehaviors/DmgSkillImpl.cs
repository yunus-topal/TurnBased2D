using UnityEngine;

namespace Models.Scriptables.SkillBehaviors
{
    [CreateAssetMenu(fileName = "NewSkillBehavior", menuName = "Skill/SkillBehavior")]
    public class DmgSkillImpl : Skill
    {
        public override void Cast(Character caster, Character target)
        {
            throw new System.NotImplementedException();
        }
    }
}