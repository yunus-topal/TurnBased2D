using Combat.UI;
using Models;
using Models.Scriptables;
using UnityEngine;

public class CharacterSelectionPanel : MonoBehaviour
{
    [SerializeField] private CharacterSO[] characters;
    public CharacterSO[] Characters => characters;

    [SerializeField] private CharacterSelector[] characterSelectors;

    public void StartGame()
    {
        var charactersToStart = new Character[characterSelectors.Length];
        // Initialize character selectors
        for (int i = 0; i < characterSelectors.Length; i++)
        {
            var index = characterSelectors[i].GetCurrentCharacterIndex();
            if (index < 0 || index >= characters.Length)
            {
                Debug.LogError($"Invalid character index {index} for selector {i}. Using default character.");
                index = 0; // Fallback to the first character if the index is invalid
            }
            charactersToStart[i] = new Character(characters[index]);

        }
        // Start the game logic here, e.g., load the combat scene
        Debug.Log("Starting game with selected characters.");
        // TODO create a save file and transition to the game scene.
    }

}
