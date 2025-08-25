using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] public Team team;
        
    }

    public static class CharacterSoExtensions
    {
        public static Character ToCharacter(this CharacterSO characterSo)
        {
            return new Character(characterSo);
        }

        public static List<Character> ToCharacters(this List<CharacterSO> characters)
        {
            return characters.Select(x => new Character(x)).ToList();
        }
    }
}
