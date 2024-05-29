using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider healthSlider;
    public void UpdateScore(int score) {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealth(int health) {
        healthSlider.value = health/100f;
    }
    
    public void IncreaseScore(int score) {
        int currentScore = int.Parse(scoreText.text.Split(' ')[1]);
        currentScore += score;
        scoreText.text = "Score: " + currentScore;
    }
}
