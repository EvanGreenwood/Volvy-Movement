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
                Vector2 dir = FlowFieldManager.Instance.GetFlowDir(rat.transform.position);
                rat.EnemyInput.Move(dir);

                _ratPositions[i] = (Vector2)rat.transform.position;
                _ratStates[i] = rat.State;
            }
        }
    }
    private void FixedUpdate()
    {
        //if (volvy == null) return;

        //var moveJob = new MoveJob()
        //{
        //    volvyPosition = (Vector2)volvy.position,
        //    ratPositions = _ratPositions,
        //    ratInputDirs = _ratInputDirs
        //};
        //moveJob.Schedule(maxNumRats, 1).Complete();

        //var attackJob = new AttackJob()
        //{
        //    volvyPosition = (Vector2)volvy.position,
        //    ratPositions = _ratPositions,
        //    ratStates = _ratStates,
        //    ratPrevAttackTimes = _ratPrevAttackTimes,
        //    ratReqDistanceToAttack = _ratReqDistanceToAttack,
        //    ratAttackCooldown = _ratAttackCooldown,
        //    currentTime = Time.time
        //};
        //attackJob.Schedule(maxNumRats, 1).Complete();
    }

    public void SpawnRat(Vector2 position)
    {
        int index = _numSpawnedRats;
        _numSpawnedRats++;

        _ratPositions[index] = position;

        _rats.Add(Instantiate(_ratPrefab, position, Quaternion.identity));
    }
}

//[BurstCompile(CompileSynchronously = true)]
//public struct AttackJob : IJobParallelFor
//{
//    [ReadOnly] public float2 volvyPosition;
//    [ReadOnly] public NativeArray<float2> ratPositions;
//    public NativeArray<RatState> ratStates;
//    public NativeArray<float> ratPrevAttackTimes;
//    public float ratReqDistanceToAttack;
//    public float ratAttackCooldown;
//    public float currentTime;

//    public void Execute(int index)
//    {
//        if (ratStates[index] == RatState.Targeting)
//        {
//            float prevAttackTime = ratPrevAttackTimes[index];
//            if (currentTime > prevAttackTime + ratAttackCooldown)
//            {
//                float2 ratPosition = ratPositions[index];
//                float sqrDist = math.distancesq(volvyPosition, ratPosition);
//                if (sqrDist < ratReqDistanceToAttack * ratReqDistanceToAttack)
//                {
//                    ratStates[index] = RatState.Attacking;
//                    ratPrevAttackTimes[index] = currentTime;
//                }
//            }
//        }
//    }
//}

//[BurstCompile(CompileSynchronously = true)]
//public struct MoveJob : IJobParallelFor
//{
//    [ReadOnly] public float2 volvyPosition;
//    [ReadOnly] public NativeArray<float2> ratPositions;
//    public NativeArray<float2> ratInputDirs;

//    public void Execute(int index)
//    {
//        float2 currRatPosition = ratPositions[index];
//        float2 dir = math.normalize(volvyPosition - currRatPosition);
//        ratInputDirs[index] = dir;
//    }
//}