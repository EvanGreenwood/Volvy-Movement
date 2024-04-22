using Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class FlowFieldManager : SingletonBehaviour<FlowFieldManager>
{
    [SerializeField] private Transform target;
    [SerializeField] private float threshDistance = 0.1f;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int numCols = 10;
    [SerializeField] private int numRows = 10;
    [SerializeField] private int batchSize = 1000;
    [SerializeField] private Vector2 offset;
    [Header("Debug")]
    [SerializeField] private bool drawFlow;

    private NativeArray<Tile> tiles;
    private NativeArray<float> distancesToTarget;
    private NativeArray<float2> flowField;
    private NativeArray<bool> visitedTiles;
    private bool isBuilt, isProcessing;
    private float maxDistance;
    private Vector2 prevTargetPosition;

    private readonly Vector2Int[] neighbours = new[]
    {
        new Vector2Int(-1,  0), // left
        new Vector2Int( 0,  1), // top
        new Vector2Int( 1,  0), // right
        new Vector2Int( 0, -1), // bottom
    };


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!isBuilt) return;

        for (int r = 0; r < numRows; ++r)
        {
            for (int c = 0; c < numCols; ++c)
            {
                int index = r * numCols + c;
                Tile tile = tiles[index];
                if (tile.IsTraversable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Vector2 pos = new Vector2(c, r) * cellSize + offset;
                Gizmos.DrawWireCube(pos, Vector2.one * cellSize);

                float distance = distancesToTarget[index];

                if (drawFlow)
                {
                    float t = 1f;//distance / maxDistance;
                    float l = (t * cellSize) / 2f;

                    Vector2 flow = math.normalize(flowField[index]) * l;
                    Gizmos.DrawRay(pos, flow);
                }
                else
                {
                    Handles.Label(pos, $"{distance}");
                }
            }
        }
    }
#endif

    protected override void Awake()
    {
        base.Awake();
        Setup();
    }
    private void Update()
    {
        if (target == null) return;

        float sqrM = ((Vector2)(target.position) - prevTargetPosition).sqrMagnitude;
        if (sqrM > (threshDistance * threshDistance) && !isProcessing)
        {
            StartCoroutine(ProcessRoutine());
        }
    }

    private void Setup()
    {
        tiles = new NativeArray<Tile>(numRows * numCols, Allocator.Persistent);
        distancesToTarget = new NativeArray<float>(numRows * numCols, Allocator.Persistent);
        flowField = new NativeArray<float2>(numRows * numCols, Allocator.Persistent);
        visitedTiles = new NativeArray<bool>(numRows * numCols, Allocator.Persistent);

        for (int r = 0; r < numRows; ++r)
        {
            for (int c = 0; c < numCols; ++c)
            {
                int index = r * numCols + c;
                tiles[index] = new Tile()
                {
                    col = c,
                    row = r,
                    type = TileType.Empty,
                    flow = float2.zero
                };
                distancesToTarget[index] = 0f;
                flowField[index] = float2.zero;
                visitedTiles[index] = false;
            }
        }
    }
    
    private IEnumerator ProcessRoutine()
    {
        isProcessing = true;

        // Build
        yield return BuildRoutine();

        // Clear
        var clearJob = new ClearJob()
        {
            visitedTiles = visitedTiles
        };
        clearJob.Schedule(numRows * numCols, 1).Complete();

        // Flow
        var flowJob = new FlowJob()
        {
            numRows = numRows,
            numCols = numCols,
            tiles = tiles,
            distancesToTarget = distancesToTarget,
            flowField = flowField
        };
        flowJob.Schedule(numRows * numCols, 1).Complete();

        prevTargetPosition = target.position;

        isProcessing = false;
    }
    private IEnumerator BuildRoutine()
    {
        Tile currentTile = GetNearesetTileAt(target.position);

        Queue<Tile> tilesToVisit = new();
        tilesToVisit.Enqueue(currentTile);
        int index = currentTile.row * numCols + currentTile.col;
        maxDistance = distancesToTarget[index] = 0f;
        visitedTiles[index] = true;

        int counter = 0;

        while (tilesToVisit.Count > 0)
        {
            currentTile = tilesToVisit.Dequeue();

            index = currentTile.row * numCols + currentTile.col;
            float currentDistance = distancesToTarget[index];

            foreach (var offset in neighbours)
            {
                int x = currentTile.col + offset.x;
                int y = currentTile.row + offset.y;
                index = y * numCols + x;

                // Check if out of bounds
                if (IsOutOfBounds(y, x))
                {
                    continue;
                }
                Tile neighbourTile = tiles[index];

                // Check if traversable
                if (!neighbourTile.IsTraversable)
                {
                    continue;
                }

                // Check if visited
                if (visitedTiles[index])
                {
                    continue;
                }

                float distance = distancesToTarget[index] = currentDistance + 1f;
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }

                tilesToVisit.Enqueue(tiles[index]);
                visitedTiles[index] = true;
            }

            counter++;

            if (counter % batchSize == 0)
            {
                yield return null;
            }
        }

        isBuilt = true;
    }

    public bool IsTraversable(int y, int x)
    {
        int index = y * numCols + x;
        return tiles[index].IsTraversable;
    }
    public bool IsOutOfBounds(int y, int x)
    {
        return (x < 0 || y < 0 || x > numCols - 1 || y > numRows - 1);
    }

    public Tile GetTile(int y, int x)
    {
        int index = y * numCols + x;
        return tiles[index];
    }
    public void SetTile(int y, int x, Tile tile)
    {
        int index = y * numCols + x;
        tiles[index] = tile;
    }

    public Vector2 GetFlow(int y, int x)
    {
        int index = y * numCols + x;
        return flowField[index];
    }
    public void SetFlow(int y, int x, Vector2 flow)
    {
        int index = y * numCols + x;
        flowField[index] = flow;
    }

    public Vector2 GetTilePosition(Tile tile)
    {
        return new Vector2(tile.col, tile.row) * cellSize + offset;
    }

    public Tile GetNearesetTileAt(Vector3 position)
    {
        int row = Mathf.Max(Mathf.Min(Mathf.FloorToInt((position.y - offset.y + (cellSize / 2f)) / cellSize), numRows - 1), 0);
        int col = Mathf.Max(Mathf.Min(Mathf.FloorToInt((position.x - offset.x + (cellSize / 2f)) / cellSize), numCols - 1), 0);
        return GetTile(row, col);
    }
    public Vector2 GetFlowAt(Vector3 position)
    {
        Tile tile = GetNearesetTileAt(position);
        return GetFlow(tile.row, tile.col);
    }
}