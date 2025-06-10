using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject savePanel;
        [SerializeField] private GameObject optionsPanel;

        private void Start()
        {
            // get player prefs
            var lastUsedSave = PlayerPrefs.GetString(Helpers.Constants.lastUsedSaveKey, string.Empty);
            // try to load the last used save file
            var lastSave = Helpers.SaveHelper.LoadSaveFileFromFileName(lastUsedSave);
            if (lastSave != null)
            {
                DisableAllPanels();
                mainPanel.SetActive(true);
            }
            else
            {
                // enable create save file panel. disable others.
                DisableAllPanels();
                savePanel.SetActive(true);
            }
        }

        public void LoadNewGameScene()
        {
            SceneManager.LoadScene("StartingMenu");
        }

        public void QuitApplication()
        {
            Application.Quit();
        } 

        private void DisableAllPanels()
        {
            mainPanel.SetActive(false);
            savePanel.SetActive(false);
            optionsPanel.SetActive(false);
        }

    }
}
