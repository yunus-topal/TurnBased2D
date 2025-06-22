using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    // TODO show more details of the selected character.
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;

        private CharacterSelectionPanel selectionPanel;
        private int currentCharacterIndex = 0;

        void Start()
        {
            selectionPanel = FindAnyObjectByType<CharacterSelectionPanel>();
            if (selectionPanel == null)
            {
                Debug.LogError("CombatTester not found in the scene.");
                return;
            }
            // Register up and down button click events
            upButton.onClick.AddListener(SelectNextChar);
            downButton.onClick.AddListener(SelectPreviousChar);
            UpdateCharacterDisplay();
        }

        public int GetCurrentCharacterIndex()
        {
            return currentCharacterIndex;
        }
        private void SelectNextChar()
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % selectionPanel.Characters.Length;
            UpdateCharacterDisplay();
        }
        private void SelectPreviousChar()
        {
            currentCharacterIndex = (currentCharacterIndex - 1 + selectionPanel.Characters.Length) % selectionPanel.Characters.Length;
            UpdateCharacterDisplay();
        }

        private void UpdateCharacterDisplay()
        {
            if (selectionPanel.Characters.Length == 0)
            {
                Debug.LogWarning("No characters available to display.");
                return;
            }
            var character = selectionPanel.Characters[currentCharacterIndex];
            characterImage.sprite = character.characterSprite;
            characterNameText.text = character.characterName;
        }

    }
}