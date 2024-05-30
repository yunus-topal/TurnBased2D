using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    [SerializeField] private int maxHealth;
    private int _health;
    [SerializeField] private float invincibilityTime;
    private float _lastHitTime;
    
    private HudManager _hudManager;
    private string _name;
    private int _attack;
    private int _defense;
    private int _speed;
    private int _level;
    private int _experience;
    private int _score;

    private void Start() {
        _hudManager = FindObjectOfType<HudManager>();
        _health = maxHealth;
    }

    public void IncreaseScore(int score) {
        _score += score;
        _hudManager.UpdateScore(_score);
    }
    
    public void TakeDamage(int damage) {
        if(Time.time - _lastHitTime < invincibilityTime) return;
        _lastHitTime = Time.time;
        _health -= damage;
        _hudManager.UpdateHealth(_health, maxHealth);
        if (_health <= 0) {
            Die();
        }
    }

    private void Die() {
        Time.timeScale = 0;
        Destroy(this);
    }
}