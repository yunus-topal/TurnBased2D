using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject _player;
    private float _speed;
    private float _health = 100;
    private HudManager _hudManager;
    public void Initialize(float speed, HudManager hudManager)
    {
        _hudManager = hudManager;
        _speed = speed;
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        if(_player == null) return;
        // find the direction to the player
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.position += direction * (Time.deltaTime * _speed);
    }
    
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            if(_player != null)
            {
                _player.GetComponent<PlayerStatus>().IncreaseScore(10);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStatus>().TakeDamage(10);
        }
    }
}
