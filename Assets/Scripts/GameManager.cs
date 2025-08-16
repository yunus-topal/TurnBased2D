using System;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject mapPanel;

    private void Start()
    {
        SetActiveCombatPanel(false);
        SetActiveMapPanel(true);
    }

    public void SetActiveCombatPanel(bool active)
    {
        combatPanel.SetActive(active);
    }

    public void SetActiveMapPanel(bool active)
    {
        mapPanel.SetActive(active);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
