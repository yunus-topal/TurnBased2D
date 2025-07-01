using System;
using Helpers;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject mapPanel;

    public void SetActiveCombatPanel(bool active)
    {
        combatPanel.SetActive(active);
    }

    public void SetActiveMapPanel(bool active)
    {
        mapPanel.SetActive(active);
    }
}
