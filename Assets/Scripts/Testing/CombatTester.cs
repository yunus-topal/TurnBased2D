using System.Collections.Generic;
using Models.Combatants;
using Models.Scriptables;
using UnityEngine;

namespace Testing
{
    public class CombatTester : MonoBehaviour
    {
        [SerializeField] private CharacterSO[] characters;

        [SerializeField] private QuickCharacterSelector[] allies;
        [SerializeField] private QuickCharacterSelector[] enemies;

        public CharacterSO[] Characters
        {
            get => characters;
        }

        public void StartCombat()
        {
            // check allies and enemies and only get active ones
            List<Character> activeAllies = new List<Character>();
            List<Character> activeEnemies = new List<Character>();

            foreach (var item in allies)
            {
                if (item.IsCharacterActive())
                {
                    activeAllies.Add(new Character(characters[item.GetCurrentCharacterIndex()]));
                }
            }
            foreach (var item in enemies)
            {
                if (item.IsCharacterActive())
                {
                    activeEnemies.Add(new Character(characters[item.GetCurrentCharacterIndex()]));
                }
            }

            // start combat controller
        }

    }

}
