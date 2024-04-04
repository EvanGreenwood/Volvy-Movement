using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnRate = 2;
    private float _spawnCounter = 0;
    [SerializeField] private float _maxY = 9;
    [SerializeField] private float _maxX = 15;
    void Start()
    {
        
    }

    //  
    void Update()
    {
        _spawnCounter += Time.deltaTime;
        if (_spawnCounter >= _spawnRate)
        {
            _spawnCounter -= _spawnRate;
            //
            if (Random.value > 0.5f)
            {
                Instantiate(_enemyPrefab, new Vector3(Random.Range(-10, 10), _maxY * (Random.Range(0, 2) * 2 - 1) - 1, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_enemyPrefab, new Vector3(_maxX * (Random.Range(0, 2) * 2 - 1), Random.Range(-7, 7), 0), Quaternion.identity);
            }
            //
            if (_spawnRate > 0.6f)
            {
                _spawnRate -= 0.03f;
            }
        }
    }
}
