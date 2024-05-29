using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour {
    private int _spawnLevel;
    private bool _isSpawning = false;
    
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    
    public void StartSpawning(int level) {
        StartCoroutine(SpawnEnemies());
    }
    
    public void SetIsSpawning(bool isSpawning) {
        _isSpawning = isSpawning;
    }

    private IEnumerator SpawnEnemies() {
        while (_isSpawning) {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
        yield return null;
    }
}
