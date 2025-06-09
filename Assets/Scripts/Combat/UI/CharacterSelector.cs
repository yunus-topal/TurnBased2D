using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.UI
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;

        //private Roster combatRoster;
        private int currentCharacterIndex = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        //void Start()
        //{
        //    combatRoster = FindAnyObjectByType<Roster>();
        //    if (combatRoster == null)
        //    {
        //        Debug.LogError("CombatTester not found in the scene.");
        //        return;
        //    }
        //    // Register up and down button click events
        //    upButton.onClick.AddListener(SelectNextChar);
        //    downButton.onClick.AddListener(SelectPreviousChar);
        //    UpdateCharacterDisplay();
        //}

        //public int GetCurrentCharacterIndex()
        //{
        //    return currentCharacterIndex;
        //}
        //private void SelectNextChar()
        //{
        //    currentCharacterIndex = (currentCharacterIndex + 1) % combatRoster.Characters.Length;
        //    UpdateCharacterDisplay();
        //}
        //private void SelectPreviousChar()
        //{
        //    currentCharacterIndex = (currentCharacterIndex - 1 + combatRoster.Characters.Length) % combatRoster.Characters.Length;
        //    UpdateCharacterDisplay();
        //}

        //private void UpdateCharacterDisplay()
        //{
        //    if (combatRoster.Characters.Length == 0)
        //    {
        //        Debug.LogWarning("No characters available to display.");
        //        return;
        //    }
        //    var character = combatRoster.Characters[currentCharacterIndex];
        //    characterImage.sprite = character.CharacterSprite;
        //    characterNameText.text = character.CharacterName;
        //}

    }
}