using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Testing
{
    public class QuickCharacterSelector : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Toggle characterToggle;
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;

        private CombatTester combatTester;
        private int currentCharacterIndex = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            combatTester = FindAnyObjectByType<CombatTester>();
            if (combatTester == null)
            {
                Debug.LogError("CombatTester not found in the scene.");
                return;
            }
            // Register up and down button click events
            upButton.onClick.AddListener(SelectNextChar);
            downButton.onClick.AddListener(SelectPreviousChar);
            UpdateCharacterDisplay();
        }

        public bool IsCharacterActive()
        {
            return characterToggle.isOn;
        }
        public int GetCurrentCharacterIndex()
        {
            return currentCharacterIndex;
        }
        private void SelectNextChar()
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % combatTester.Characters.Length;
            UpdateCharacterDisplay();
        }
        private void SelectPreviousChar()
        {
            currentCharacterIndex = (currentCharacterIndex - 1 + combatTester.Characters.Length) % combatTester.Characters.Length;
            UpdateCharacterDisplay();
        }

        private void UpdateCharacterDisplay()
        {
            if (combatTester.Characters.Length == 0)
            {
                Debug.LogWarning("No characters available to display.");
                return;
            }
            var character = combatTester.Characters[currentCharacterIndex];
            characterImage.sprite = character.CharacterSprite;
            characterNameText.text = character.CharacterName;
        }

    }
}
