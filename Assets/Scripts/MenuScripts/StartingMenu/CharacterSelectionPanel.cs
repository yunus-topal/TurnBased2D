using Helpers;
using Models;
using Models.Scriptables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts.StartingMenu
{
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
            Debug.Log("Starting game with selected characters.");

            // save to persistent storage
            SaveHelper.UpdateSaveFile(charactersToStart);
            
            Constants.SetRngWithSeed(SaveHelper.CurrentSaveFile.SeedNumber);
            
            // load game scene
            SceneManager.LoadScene("GameScene");
        }

    }
}
