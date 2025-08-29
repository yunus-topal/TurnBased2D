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
        
        private Character _character;
        private TurnManager _turnManager;

        public event Action<Character> Clicked;

        public void Initialize(Character character)
        {
            _turnManager = FindAnyObjectByType<TurnManager>();
            _character = character;
            SetCharacter(character.Sprite, character.Name, character.MaxHealth);
            CharacterButton.onClick.AddListener(OnCharacterButtonClick);

            if (_turnManager is null)
            {
                Debug.LogError($"{nameof(CombatCharacterUIHelper)}.{nameof(Initialize)}: TurnManager is null");
                return;
            }

            Clicked += _turnManager.SetSelectedTarget;

        }
        
        private void SetCharacter(Sprite sprite, string charName, int maxHealth) {
            Sprite.sprite = sprite;
            Name.text = charName;
            HealthText.text = $"{maxHealth}/{maxHealth}";
            HealthBar.maxValue = maxHealth;
            HealthBar.value = maxHealth;
            HealthBarImage.color = Color.green;
        }
        
        public void UpdateHealth(int health) {
            HealthText.text = $"{health}/{HealthBar.maxValue}";
            HealthBar.value = health;
            HealthBarImage.color = Color.Lerp(Color.red, Color.green, health / HealthBar.maxValue);
        }

        // callback to TurnManager
        private void OnCharacterButtonClick()
        {
            Clicked?.Invoke(_character);
        }

        private void OnDestroy()
        {
            //Clicked -= _turnManager.SetSelectedTarget;
        }
    }
}
