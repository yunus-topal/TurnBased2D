using Models;
using UnityEngine;

namespace Models.Scriptables.SkillBehaviors
{
    [CreateAssetMenu(fileName = "Fireball", menuName = "Scriptable Objects/Skill/Fireball")]
    public class Fireball : Skill
    {
        public override void Cast(Character caster, Character target)
        {
            Debug.Log($"Casting {SkillName} on {target.Name} by {caster.Name}.");
        }
    }
}