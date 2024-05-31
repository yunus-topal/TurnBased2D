using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    [SerializeField] private int maxHealth;
    private int _health;
    [SerializeField] private float invincibilityTime;
    private float _lastHitTime;
    
    private RoundManager _roundManager;
    private string _name;
    private int _attack;
    private int _defense;
    private int _speed;
    private int _level;
    private int _experience;

    private void Start() {
        _roundManager = FindObjectOfType<RoundManager>();
        _health = maxHealth;
    }
    
    public void TakeDamage(int damage) {
        if(Time.time - _lastHitTime < invincibilityTime) return;
        _lastHitTime = Time.time;
        _health -= damage;
        _roundManager.UpdateHealth(_health, maxHealth);
        if (_health <= 0) {
            Die();
        }
    }

    private void Die() {
        Time.timeScale = 0;
        Destroy(this);
    }
}