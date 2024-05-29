using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameMenu;
    void Update() {
        // if player presses escape key, pause the game and show the menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0;
            gameMenu.gameObject.SetActive(true);
        }
    }
    
    public void ResumeGame(){
        Time.timeScale = 1;
        gameMenu.gameObject.SetActive(false);
    }
    public void QuitGame(){
        Application.Quit();
    }
}
