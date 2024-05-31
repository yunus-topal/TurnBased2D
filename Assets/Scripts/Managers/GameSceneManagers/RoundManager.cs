using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject preparationPanel;

    private int _score;
    public void IncreaseScore(int score) {
        _score += score;
        UpdateScore(_score);
    }
    public void UpdateScore(int score) {
        scoreText.text = "Score: " + score;
        // finish round and save character experience.
        if (score > 200) {
            EndRound();
        }
    }

    public void UpdateHealth(int health, int maxHealth) {
        healthSlider.value = health/(float)maxHealth;
    }

    private void EndRound() {
        Time.timeScale = 0;
        enemySpawner.SetIsSpawning(false);
        // kill all remaining enemies
        foreach (var enemy in FindObjectsOfType<EnemyMovement>()) {
            Destroy(enemy.gameObject);
        }
        // kill player
        Destroy(FindObjectOfType<PlayerStatus>().gameObject);
        // show preparation panel
        preparationPanel.SetActive(true);
        // TODO: save character experiences
    }
}
