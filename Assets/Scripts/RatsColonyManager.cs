using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RatsColonyManager : MonoBehaviour
{
    [SerializeField] private Rat _ratPrefab;
    [SerializeField] private float _spawnRate = 2;
    [SerializeField] private float _maxY = 9;
    [SerializeField] private float _maxX = 15;
    [SerializeField] private int maxNumRats = 100;
    [SerializeField] private Transform[] initialSpawnPoints;
    [SerializeField] private Transform volvy;
    [SerializeField] private float _ratSpeed = 3f;
    [SerializeField] private float _ratReqDistanceToAttack = 0.1f;
    [SerializeField] private float _ratAttackCooldown = 1f;

    private List<Rat> _rats = new();
    private int _numSpawnedRats = 0;
    private float _spawnCounter = 0;

    private NativeArray<float> _ratPrevAttackTimes;
    private NativeArray<float2> _ratInputDirs;
    private NativeArray<float2> _ratPositions;
    private NativeArray<RatState> _ratStates;


    private void Start()
    {
        _ratPrevAttackTimes = new NativeArray<float>(maxNumRats, Allocator.Persistent);
        _ratInputDirs = new NativeArray<float2>(maxNumRats, Allocator.Persistent);
        _ratPositions = new NativeArray<float2>(maxNumRats, Allocator.Persistent);
        _ratStates = new NativeArray<RatState>(maxNumRats, Allocator.Persistent);

        for (int i = 0; i < maxNumRats; i++)
        {
            _ratPrevAttackTimes[i] = 0f;
            _ratStates[i] = RatState.Targeting;
        }

        foreach (var spawnPoint in initialSpawnPoints)
        {
            SpawnRat(spawnPoint.position);
        }
    }
    private void OnDestroy()
    {
        _ratPrevAttackTimes.Dispose();
        _ratInputDirs.Dispose();
        _ratPositions.Dispose();
        _ratStates.Dispose();
    }

    private void Update()
    {
        _spawnCounter += Time.deltaTime;
        if (_spawnCounter >= _spawnRate)
        {
            _spawnCounter -= _spawnRate;

            var pos = Vector3.zero;
            if (Random.value > 0.5f)
            {
                pos = new Vector3(Random.Range(-10, 10), _maxY * (Random.Range(0, 2) * 2 - 1) - 1, 0);
            }
            else
            {
                pos = new Vector3(_maxX * (Random.Range(0, 2) * 2 - 1), Random.Range(-7, 7), 0);
            }
            SpawnRat(pos);

            if (_spawnRate > 0.6f)
            {
                _spawnRate -= 0.03f;
            }
        }


        for (int i = 0; i < _rats.Count; i++)
        {
            var rat = _rats[i];
            if (rat == null)
            {
                _rats.RemoveAt(i);
                i--;
            }
            else
            {
                Vector2 dir = FlowFieldManager.Instance.GetFlowAt(rat.transform.position);
                rat.EnemyInput.Move(dir);

                _ratPositions[i] = (Vector2)rat.transform.position;
                _ratStates[i] = rat.State;
            }
        }
    }

    public void SpawnRat(Vector2 position)
    {
        int index = _numSpawnedRats;
        _numSpawnedRats++;

        _ratPositions[index] = position;

        _rats.Add(Instantiate(_ratPrefab, position, Quaternion.identity));
    }
}