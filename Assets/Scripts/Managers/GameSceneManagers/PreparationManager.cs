using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PreparationManager : MonoBehaviour {
    [SerializeField] private GameObject baseMenuPanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject preparationPanel;
    // When I change this type to Image, it becomes null on play mode.
    [SerializeField] private GameObject[] characterImages;
    
    private EnemySpawner _enemySpawner;
    private int _level = 0;

    private void Start() {
        Time.timeScale = 0;
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void IncreaseLevel() {
        _level++;
        levelText.text = "Level: " + _level;
    }
    
    public void DecreaseLevel() {
        _level = Mathf.Max(0, _level - 1);
        levelText.text = "Level: " + _level;
    }
    
    public void StartGame() {
        preparationPanel.SetActive(false);
        Time.timeScale = 1;
        _enemySpawner.StartSpawning(_level);
    }

    public void ReturnToBaseMenu() {
        baseMenuPanel.SetActive(true);
    }

    public void SetCharacterIcons(DraggableButton[] buttons) {
        for (int i = 0; i < characterImages.Length; i++) {
            if (buttons[i] == null) continue;
            characterImages[i].GetComponent<Image>().sprite = buttons[i].gameObject.GetComponent<Image>().sprite;
        }
    }
}
