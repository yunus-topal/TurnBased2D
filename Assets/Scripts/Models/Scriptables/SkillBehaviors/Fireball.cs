using Models.Combatants;
using Models.Scriptables;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Scriptable Objects/Fireball")]
public class Fireball : Skill
{
    public override void Cast(Character caster, Character target)
    {
        Debug.Log($"Casting {SkillName} on {target.CharacterName} by {caster.CharacterName}.");
    }
}
