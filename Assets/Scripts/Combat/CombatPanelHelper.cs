using System;
using System.Collections.Generic;
using Combat.UI;
using Helpers;
using Models;
using Models.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Combat
{
    public class CombatPanelHelper : MonoBehaviour
    {
        [SerializeField] private List<CombatCharacterUIHelper> enemyCharacterUIs  = new ();
        [SerializeField] private List<CombatCharacterUIHelper> playerCharacterUIs  = new ();
        [SerializeField] private TextMeshProUGUI turnLabel;
        [SerializeField] private SkillUIHelper skillUIHelper;
        [SerializeField] private GameObject combatSummaryObject;
        [SerializeField] private TextMeshProUGUI combatSummaryText;
        [SerializeField] private Button combatSummaryButton;

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

        private void OnEnable()
        {
            SetupCombatUI(true);
        }

        public void Initialize(List<Character> characters, List<Character> enemies)
        {
            if (characters == null) throw new ArgumentNullException(nameof(characters));
            
            int playerCount = GeneralHelper.ClampAndWarn(characters.Count, _playerLimit, "Player");
            int enemyCount  = GeneralHelper.ClampAndWarn(enemies.Count,     _enemyLimit,  "Enemy");

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

        #endregion

        #region Label

        public void SetTurnLabel(string label, int ttl = 2)
        {
            turnLabel.gameObject.SetActive(true);
            turnLabel.text = $"{label}'s Turn";
            // after ttl seconds, hide label. if -1, do not hide
            if (ttl >= 0)
            {
                CancelInvoke(nameof(HideLabel));
                Invoke(nameof(HideLabel), ttl);
            }
        }

        Dictionary<StatusEffectType, string> StatusLabels = new Dictionary<StatusEffectType, string>()
        {
            { StatusEffectType.Stun, "stunned" },
            { StatusEffectType.Sleep, "sleeping" },
        };
        public void SetTurnLabelIncapacitated(string label, StatusEffectType statusEffectType, int ttl = 2)
        {
            var statusLabel = StatusLabels.GetValueOrDefault(statusEffectType, "incapacitated");
            
            turnLabel.gameObject.SetActive(true);
            turnLabel.text = $"{label} is {statusLabel}";
            if (ttl >= 0)
            {
                CancelInvoke(nameof(HideLabel));
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

        public void ShowCombatSummary(bool victory, int gold, UnityAction proceedAction)
        {
            SetupCombatUI(false);
            combatSummaryText.text = victory ? $"Victory!\nYou earned {gold} gold." : "Defeat";
            combatSummaryButton.onClick.RemoveAllListeners();
            combatSummaryButton.onClick.AddListener(proceedAction);
        }

        private void SetupCombatUI(bool combat)
        {
            skillUIHelper.gameObject.SetActive(combat);
            foreach (var p in playerCharacterUIs)
            {
                p.gameObject.SetActive(combat);
            }
            foreach (var p in enemyCharacterUIs)
            {
                p.gameObject.SetActive(combat);
            }
            combatSummaryObject.SetActive(!combat);
        }
    }
}
