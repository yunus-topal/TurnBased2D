using Models;
using UnityEngine;

namespace MenuScripts.MainMenu
{
    public class SaveFileUIHelper : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI saveNameText;
        private MainMenuManager  mainMenuManager;
        private SaveFile SaveFile { get; set; }
        public void Initialize(SaveFile saveFile, GameObject mainPanel, GameObject savePanel)
        {
            mainMenuManager = FindAnyObjectByType<MainMenuManager>();
            Debug.Log("save file name: " + saveFile.SaveName);
            SaveFile = saveFile;
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnSaveFileSelected(mainPanel, savePanel));
            saveNameText.text = saveFile.SaveName;
            // TODO add more fields to prefab and populate here accordingly.

        }

        private void OnSaveFileSelected(GameObject  mainPanel, GameObject savePanel)
        {
            // Save the selected save file name to PlayerPrefs
            PlayerPrefs.SetString(Helpers.Constants.lastUsedSaveKey, SaveFile.SaveName);
            PlayerPrefs.Save();

            // Optionally, you can load the game or perform other actions here
            Debug.Log($"Selected save file: {SaveFile.SaveName}");

            // Activate main panel, disable savePanel
            savePanel.SetActive(false);
            mainPanel.SetActive(true);
            mainMenuManager.CheckSaveFiles();
        }
    }
}
