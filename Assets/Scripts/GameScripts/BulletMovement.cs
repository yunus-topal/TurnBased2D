using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

    private float _timeToLive = 1f;
    private float _speed;
    private Vector3 _direction = Vector3.zero;
    public void Initialize(Vector3 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }
    
    void Update()
    {
        transform.position += _direction * (Time.deltaTime * _speed);
        _timeToLive -= Time.deltaTime;
        if (_timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyMovement>().TakeDamage(50);
            Destroy(gameObject);
        }
    }
}
