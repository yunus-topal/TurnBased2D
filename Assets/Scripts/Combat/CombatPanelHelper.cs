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
            if (characters.Count > _playerLimit)
                Debug.LogWarning($"Player Character Limit Exceeded, Only first {_playerLimit} characters will be used.");
            if (enemyGroup.characters.Count > _enemyLimit)
                Debug.LogWarning($"Enemy Character Limit Exceeded, Only first {_enemyLimit} characters will be used.");

            // initialize players
            for (int i = 0; i < Math.Min(_playerLimit, characters.Count); i++)
            {
                playerCharacterUIs[i].Initialize(characters[i]);
            }
            // initialize enemies
            for (int i = 0; i < Math.Min(_enemyLimit,enemyGroup.characters.Count); i++)
            {
                enemyCharacterUIs[i].Initialize(new Character(enemyGroup.characters[i]));
            } 
        }
    }
}
