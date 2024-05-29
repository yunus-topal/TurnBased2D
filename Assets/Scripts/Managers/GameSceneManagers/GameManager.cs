using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameStartPanel;
    
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
        gameStartPanel.SetActive(false);
        Time.timeScale = 1;
        _enemySpawner.StartSpawning(_level);
    }
}
