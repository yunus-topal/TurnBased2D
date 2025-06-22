using Models;
using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    public class CreateSaveMenuManager : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button createButton;
        [SerializeField] private GameObject saveFilePrefab;
        [SerializeField] private Transform saveFileListContainer;

        [SerializeField] private GameObject CreateSaveInput;

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject savePanel;
        private void OnEnable()
        {
            CreateSaveInput.SetActive(false);
            // check if there is a valid save file in the player prefs.
            var lastUsedSave = PlayerPrefs.GetString(Helpers.Constants.lastUsedSaveKey, string.Empty);
            // try to load the last used save file
            var lastSave = Helpers.SaveHelper.LoadSaveFileFromFileName(lastUsedSave);
            if (lastSave == null)
            {
                backButton.gameObject.SetActive(false);
            }

            SetupSaveScrollView();
        }

        public void CreateNewSave()
        {
            var saveName = CreateSaveInput.GetComponentInChildren<TMPro.TMP_InputField>().text;
            if (string.IsNullOrEmpty(saveName))
            {
                Debug.LogError("Save name cannot be empty.");
                return;
            }
            // Create a new save file with the given name
            var newSaveFile = new SaveFile { SaveName = saveName };
            // Save the new save file to disk
            Helpers.SaveHelper.SaveNewSaveFile(newSaveFile);
        }

        public void SetupSaveScrollView()
        {
            // Clear existing save file entries
            foreach (Transform child in saveFileListContainer)
            {
                Destroy(child.gameObject);
            }
            // Get all save files
            var saveFiles = Helpers.SaveHelper.GetAllSaveFiles();
            // Populate the scroll view with save files
            foreach (var saveFile in saveFiles)
            {
                var saveFileObject = Instantiate(saveFilePrefab, saveFileListContainer);
                // set parent
                var saveFileUIHelper = saveFileObject.GetComponent<SaveFileUIHelper>();
                if (saveFileUIHelper != null)
                {
                    saveFileUIHelper.Initialize(saveFile, mainPanel, savePanel);
                }
            }
        }

    }
}
