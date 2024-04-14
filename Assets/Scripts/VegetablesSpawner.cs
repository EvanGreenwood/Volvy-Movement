using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetablesSpawner : SingletonBehaviour<VegetablesSpawner> 
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
            TrySpawnVegetable();
        }
    }

    private void TrySpawnVegetable()
    {
        int attempts = 0;
        while (attempts < 25)
        {
            Vector3 pos = new Vector3((Random.Range(4, 21)) * (Random.Range(0, 2) * 2 - 1), (Random.Range(4, 14)) * (Random.Range(0, 2) * 2 - 1) - 1, 0);
            if (  Physics2D.OverlapCircle(pos, 2.5f, 1 << Layer.DontRender) == null && !Physics.CheckSphere(pos, 1, 1 << Layer.RootedVegetables))
            {
                Instantiate(_vegetablePrefab, pos, Quaternion.identity);
                break;
            }
           
        }
    }

    public void SpawnVegetable(VegetableType type, Vector2 pos)
    {
        Instantiate(_vegetablePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

    }
}
