using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject[] otherPanels;

        private void Start()
        {
            mainPanel.SetActive(true);
            foreach (var panel in otherPanels)
            {
                panel.SetActive(false);
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

        public void FetchSaveFiles()
        {
            // fetch all the save files from the save file location
        }

    }
}
