using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.UI
{
    public class CombatCharacterUIHelper : MonoBehaviour
    {
        [SerializeField] private Image Sprite;
        [SerializeField] private TMP_Text Name;
        [SerializeField] private TMP_Text HealthText;
        [SerializeField] private Slider HealthBar;
        [SerializeField] private Image HealthBarImage;
        [SerializeField] private Button CharacterButton;
        [SerializeField] private GameObject statusIconParent;
        [SerializeField] private GameObject statusIcon;
        
        private Character _character;
        private TurnManager _turnManager;

        public event Action<Character> Clicked;

        public void Initialize(Character character)
        {
            _turnManager = FindAnyObjectByType<TurnManager>();
            _character = character;
            SetCharacter();
            CharacterButton.onClick.RemoveAllListeners();
            CharacterButton.onClick.AddListener(OnCharacterButtonClick);

            if (_turnManager is null)
            {
                Debug.LogError($"{nameof(CombatCharacterUIHelper)}.{nameof(Initialize)}: TurnManager is null");
                return;
            }

            Clicked += _turnManager.SetSelectedTarget;

        }
        
        private void SetCharacter() {
            Sprite.sprite = _character.Sprite;
            Name.text = _character.Name;
            HealthText.text = $"{_character.CurrentHealth}/{_character.MaxHealth}";
            HealthBar.maxValue = _character.MaxHealth;
            HealthBar.value = _character.CurrentHealth;
            HealthBarImage.color = Color.green;
            SetStatusIcons();
        }
        
        public void UpdateCharacter(Character character) {
            HealthText.text = $"{character.CurrentHealth}/{HealthBar.maxValue}";
            HealthBar.value = character.CurrentHealth;
            HealthBarImage.color = Color.Lerp(Color.red, Color.green, character.CurrentHealth / HealthBar.maxValue);
            SetStatusIcons();
        }

        // callback to TurnManager
        private void OnCharacterButtonClick()
        {
            Clicked?.Invoke(_character);
        }

        private void SetStatusIcons()
        {
            // clear status icon parent.
            foreach (Transform child in statusIconParent.transform)
            {
                Destroy(child.gameObject);
            }
            // spawn status icon helper for each.
            foreach (var activeEffect in _character.ActiveEffects)
            {
                var newEffect = Instantiate(statusIcon, statusIconParent.transform);
                newEffect.GetComponent<StatusEffectIconHelper>().Initialize(activeEffect, _turnManager.GetStatusEffectIconEntry(activeEffect));
            }
        }
    }
}
