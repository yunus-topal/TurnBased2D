using System.Collections.Generic;
using UnityEngine;

namespace Models.Scriptables {
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/CharacterScriptable")]
    public class CharacterSO : ScriptableObject
    {
        // health will be automatically calculated from combat stats.
        [SerializeField] public string characterName;
        [SerializeField] public int level;
        [SerializeField] public Sprite characterSprite;
        [SerializeField] public CombatStats combatStats;
        [SerializeField] public List<Equipment> equipments;
        [SerializeField] public List<Skill> skills;
        
    }
}
