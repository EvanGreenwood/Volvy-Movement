using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetablesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _vegetablePrefab;
    [SerializeField] private float _spawnRate = 2;
    private float _spawnCounter = 0;
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
            Instantiate(_vegetablePrefab, new Vector3((Random.Range(4, 11)) * (Random.Range(0, 2) * 2 - 1), (Random.Range(2, 7)) * (Random.Range(0, 2) * 2 - 1) - 1, 0), Quaternion.identity);
        }
    }
}
