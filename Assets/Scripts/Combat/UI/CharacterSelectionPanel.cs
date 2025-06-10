using System.Linq;
using Combat.UI;
using Helpers;
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
        // Serialize 4 characters and 

        SaveFile saveFile = new SaveFile
        {
            Characters = charactersToStart.Select(c => c.ToData()).ToArray(),
            SaveName = "New Game",
            //SaveDate = System.DateTime.Now
        };

        // save to persistent storage

    }

}
