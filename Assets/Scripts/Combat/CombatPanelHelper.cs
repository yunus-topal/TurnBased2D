using System;
using System.Collections.Generic;
using Combat.UI;
using Models;
using Models.Scriptables;
using TMPro;
using UnityEngine;

namespace Combat
{
    public class CombatPanelHelper : MonoBehaviour
    {
        [SerializeField] private List<CombatCharacterUIHelper> enemyCharacterUIs  = new ();
        [SerializeField] private List<CombatCharacterUIHelper> playerCharacterUIs  = new ();
        [SerializeField] private TextMeshProUGUI turnLabel;
        [SerializeField] private SkillUIHelper skillUIHelper;

        private EnemyGroup _enemyGroup;
        private List<Character> _characters = new List<Character>();

        private int _enemyLimit = 0;
        private int _playerLimit = 0;

        #region Init

        private void Awake()
        {
            _enemyLimit = enemyCharacterUIs.Count;
            _playerLimit = playerCharacterUIs.Count;
        }

        public void Initialize(List<Character> characters, List<Character> enemies)
        {
            if (characters == null) throw new ArgumentNullException(nameof(characters));
            
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
                if (active) enemyCharacterUIs[i].Initialize(enemies[i]);
            }
        }
        
        private static int ClampAndWarn(int actual, int limit, string label)
        {
            if (actual > limit)
                Debug.LogWarning($"{label} Character Limit exceeded; only first {limit} will be used.");
            return Math.Min(actual, limit);
        }


        #endregion

        #region Label

        public void SetTurnLabel(string label, int ttl = 2)
        {
            turnLabel.gameObject.SetActive(true);
            turnLabel.text = $"{label}'s Turn";
            // after ttl seconds, hide label. if -1, do not hide
            if (ttl >= 0)
            {
                Invoke(nameof(HideLabel), ttl);
            }
            
            
        }

        private void HideLabel()
        {
            turnLabel.gameObject.SetActive(false);
        }

        #endregion

        public void UpdateCharUIs(List<Character> players, List<Character> enemies)
        {
            for (var i = 0; i < players.Count; i++)
            {
                playerCharacterUIs[i].UpdateCharacter(players[i]);
            }
            for (var i = 0; i < enemies.Count; i++)
            {
                enemyCharacterUIs[i].UpdateCharacter(enemies[i]);
            }
        }
    }
}
