using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject mapPanel;
    private MapNodeHelper _currentMapNode;
    public MapNodeHelper CurrentMapNode => _currentMapNode;

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

    public void SetCurrentMapNode(MapNodeHelper mapNode)
    {
        _currentMapNode = mapNode;
    }
    
    // TODO: after player beats the game.
    public void HandleVictory(){}
    
}
