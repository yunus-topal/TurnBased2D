using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rest
{
    public class RestCharacterUIHelper : MonoBehaviour
    {
        [SerializeField] private Image Sprite;
        [SerializeField] private TMP_Text Name;
        [SerializeField] private TMP_Text HealthText;
        [SerializeField] private Slider HealthBar;
        [SerializeField] private Image HealthBarImage;
        [SerializeField] private Button CharacterButton;
        // can this be a list?
        [SerializeField] private Toggle restToggle;
        [SerializeField] private Toggle upgradeToggle;
        
        private Character _character;

        public void Initialize(Character character)
        {
            _character = character;
            SetCharacter();
            CharacterButton.onClick.RemoveAllListeners();
            // toggle rest by default
            restToggle.isOn = true;
            upgradeToggle.isOn = false;
        }
        
        private void SetCharacter() {
            Sprite.sprite = _character.Sprite;
            Name.text = _character.Name;
            HealthText.text = $"{_character.CurrentHealth}/{_character.MaxHealth}";
            HealthBar.maxValue = _character.MaxHealth;
            HealthBar.value = _character.CurrentHealth;
            HealthBarImage.color = Color.green;
        }
        
        public void UpdateCharacter(Character character) {
            HealthText.text = $"{character.CurrentHealth}/{HealthBar.maxValue}";
            HealthBar.value = character.CurrentHealth;
            HealthBarImage.color = Color.Lerp(Color.red, Color.green, character.CurrentHealth / HealthBar.maxValue);
        }

        public void OnRestToggleValueChanged(bool isOn)
        {
            if (isOn) upgradeToggle.isOn = false;
            
        }

        public void OnUpgradeToggleValueChanged(bool isOn)
        {
            if (isOn) restToggle.isOn = false;  
        }
    }
}
