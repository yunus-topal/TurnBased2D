using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject restPanel;
    private MapNodeHelper _currentMapNode;
    public MapNodeHelper CurrentMapNode => _currentMapNode;
    
    private List<GameObject> panels = new List<GameObject>();

    private void Start()
    {
        panels.Add(combatPanel);
        panels.Add(mapPanel);
        panels.Add(restPanel);
        ActivateMapPanel();
    }
    
    public void ActivateCombatPanel()
    {
        foreach (var panel in panels)
            panel.SetActive(false);
        combatPanel.SetActive(true);
    }

    public void ActivateMapPanel()
    {
        foreach (var panel in panels)
            panel.SetActive(false);
        mapPanel.SetActive(true);
    }

    public void ActivateRestPanel()
    {
        foreach (var panel in panels)
            panel.SetActive(false);
        restPanel.SetActive(true);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetCurrentMapNode(MapNodeHelper mapNode)
    {
        _currentMapNode = mapNode;
    }
    
    // TODO: after player beats the game.
    public void HandleVictory(){}
    
}
