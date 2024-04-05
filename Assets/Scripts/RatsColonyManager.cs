using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RatsColonyManager : MonoBehaviour
{
    [SerializeField] private Character _ratPrefab;
    [SerializeField] private float _spawnRate = 2;
    private float _spawnCounter = 0;
    [SerializeField] private float _maxY = 9;
    [SerializeField] private float _maxX = 15;
    [SerializeField] private int maxNumRats = 100;
    [SerializeField] private Transform volvy;
    [SerializeField] private float _ratSpeed = 3f;
    [SerializeField] private float _ratDistanceToAttack = 0.1f;
    [SerializeField] private float _ratAttackCooldown = 1f;
    [SerializeField] private Transform[] initialSpawnPoints;

    private List<Character> _rats = new();
    private int _numSpawnedRats = 0;

    private NativeArray<float> _prevAttackTimes;
    private NativeArray<float2> _ratInputVectors;
    private NativeArray<float2> _ratPositions;
    private NativeArray<RatState> _ratStates;


    private void Start()
    {
        _prevAttackTimes = new NativeArray<float>(maxNumRats, Allocator.Persistent);
        _ratInputVectors = new NativeArray<float2>(maxNumRats, Allocator.Persistent);
        _ratPositions = new NativeArray<float2>(maxNumRats, Allocator.Persistent);
        _ratStates = new NativeArray<RatState>(maxNumRats, Allocator.Persistent);

        for (int i = 0; i < maxNumRats; i++)
        {
            _prevAttackTimes[i] = 0f;
            _ratStates[i] = RatState.Targeting;
        }

        foreach (var spawnPoint in initialSpawnPoints)
        {
            SpawnRat(spawnPoint.position);
        }
    }
    private void OnDestroy()
    {
        _prevAttackTimes.Dispose();
        _ratInputVectors.Dispose();
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
            (_rats[i].Input as EnemyInput).Move(_ratInputVectors[i]);
            
            if (_ratStates[i] == RatState.Attacking)
            {
                Debug.Log(i + " ATTACK");
                _ratStates[i] = RatState.Targeting;
            }
        }
    }
    private void FixedUpdate()
    {
        //var pathfindJob = new PathfindJob();
        //pathfindJob.Schedule(maxNumRats, 1);

        var moveJob = new MoveJob()
        {
            ratPositions = _ratPositions,
            ratInputVectors = _ratInputVectors,
            volvyPosition = (Vector2)volvy.position
        };
        moveJob.Schedule(maxNumRats, 1).Complete();

        var attackJob = new AttackJob()
        {
            prevAttackTimes = _prevAttackTimes,
            volvyPosition = (Vector2)volvy.position,
            currentTime = Time.time,
            distanceToAttack = _ratDistanceToAttack,
            ratPositions = _ratInputVectors,
            ratStates = _ratStates
        };
        attackJob.Schedule(maxNumRats, 1).Complete();
    }

    public void SpawnRat(Vector2 position)
    {
        int index = _numSpawnedRats;
        _numSpawnedRats++;

        _ratPositions[index] = position;

        _rats.Add(Instantiate(_ratPrefab, position, Quaternion.identity));
    }
}


[BurstCompile(CompileSynchronously = true)]
public struct AttackJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> ratPositions;
    [ReadOnly] public float2 volvyPosition;
    public NativeArray<float> prevAttackTimes;
    public float distanceToAttack;
    public float attackCooldown;
    public NativeArray<RatState> ratStates;
    public float currentTime;

    public void Execute(int index)
    {
        if (ratStates[index] != RatState.Targeting) return; // must be targeting the player to attack

        float prevAttackTime = prevAttackTimes[index];
        if (currentTime > prevAttackTime + attackCooldown)
        {
            float2 ratPosition = ratPositions[index];
            float sqrDist = math.distancesq(volvyPosition, ratPosition);
            if (sqrDist < distanceToAttack * distanceToAttack)
            {
                ratStates[index] = RatState.Attacking;
                prevAttackTimes[index] = currentTime;
            }
        }
    }
}

[BurstCompile(CompileSynchronously = true)]
public struct MoveJob : IJobParallelFor
{
    public NativeArray<float2> ratPositions;
    public NativeArray<float2> ratInputVectors;
    [ReadOnly] public float2 volvyPosition;

    public void Execute(int index)
    {
        float2 currRatPosition = ratPositions[index];
        float2 dir = math.normalize(volvyPosition - currRatPosition);
        ratInputVectors[index] = dir;
    }
}

[BurstCompile(CompileSynchronously = true)]
public struct PathfindJob : IJobParallelFor
{
    public void Execute(int index)
    {
    }
}


public enum RatState
{
    Targeting,
    Attacking,
    Stunned,
    Dying
}