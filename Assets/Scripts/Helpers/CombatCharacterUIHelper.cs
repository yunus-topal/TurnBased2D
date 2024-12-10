using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers {
    public class CombatCharacterUIHelper : MonoBehaviour {
        [SerializeField] private Image Sprite;
        [SerializeField] private TMP_Text Name;
        [SerializeField] private TMP_Text HealthText;
        [SerializeField] private Slider HealthBar;
        
        public void SetCharacter(Sprite sprite, string charName, int maxHealth) {
            Sprite.sprite = sprite;
            Name.text = charName;
            HealthText.text = $"{maxHealth}/{maxHealth}";
            HealthBar.maxValue = maxHealth;
            HealthBar.value = maxHealth;
            Sprite.color = Color.green;
        }
        
        public void UpdateHealth(int health) {
            HealthText.text = $"{health}/{HealthBar.maxValue}";
            HealthBar.value = health;
            Sprite.color = Color.Lerp(Color.red, Color.green, health / HealthBar.maxValue);
        }
    }
}
