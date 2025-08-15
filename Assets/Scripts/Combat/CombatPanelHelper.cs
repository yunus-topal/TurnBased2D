using System;
using System.Collections.Generic;
using Combat.UI;
using Models;
using Models.Scriptables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    public class CombatPanelHelper : MonoBehaviour
    {
        [SerializeField] private List<CombatCharacterUIHelper> enemyCharacterUIs  = new ();
        [SerializeField] private List<CombatCharacterUIHelper> playerCharacterUIs  = new ();

        private EnemyGroup _enemyGroup;
        private List<Character> _characters = new List<Character>();

        private int _enemyLimit = 0;
        private int _playerLimit = 0;

        private void Awake()
        {
            _enemyLimit = enemyCharacterUIs.Count;
            _playerLimit = playerCharacterUIs.Count;
        }

        public void Initialize(List<Character> characters, EnemyGroup enemyGroup)
        {
            if (characters == null) throw new ArgumentNullException(nameof(characters));
            if (enemyGroup == null) throw new ArgumentNullException(nameof(enemyGroup));

            var enemies = enemyGroup.Characters; // use a single canonical property

            int playerCount = ClampAndWarn(characters.Count, _playerLimit, "Player");
            int enemyCount  = ClampAndWarn(enemies.Count,     _enemyLimit,  "Enemy");

            // Players
            for (int i = 0; i < playerCharacterUIs.Count; i++)
            {
                bool active = i < playerCount;
                playerCharacterUIs[i].gameObject.SetActive(active);
                if (active) playerCharacterUIs[i].Initialize(characters[i]);
            }

            // Enemies
            for (int i = 0; i < enemyCharacterUIs.Count; i++)
            {
                bool active = i < enemyCount;
                enemyCharacterUIs[i].gameObject.SetActive(active);
                if (active) enemyCharacterUIs[i].Initialize(new Character(enemies[i]));
            }
        }
        
        private static int ClampAndWarn(int actual, int limit, string label)
        {
            if (actual > limit)
                Debug.LogWarning($"{label} Character Limit exceeded; only first {limit} will be used.");
            return Math.Min(actual, limit);
        }
    }
}
