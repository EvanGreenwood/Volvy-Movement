using Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlowFieldManager : SingletonBehaviour<FlowFieldManager>
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int numCols = 10;
    [SerializeField] private int numRows = 10;
    [SerializeField] private Transform target;
    [SerializeField] private bool drawFlow;

    private Tile[][] tiles;
    private bool[][] visitedTiles;
    private float[][] distancesToTarget;
    private Vector2[][] flowField;
    private bool isBuilt;
    private float maxDistance;

    private readonly Vector2Int[] neighbours = new[]
    {
        //new Vector2Int(-1, -1), // bottom-left
        new Vector2Int(-1,  0), // left
        //new Vector2Int(-1,  1), // top-left
        new Vector2Int( 0,  1), // top
        //new Vector2Int( 1,  1), // top-right
        new Vector2Int( 1,  0), // right
        //new Vector2Int( 1, -1), // bottom-right
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
                Tile tile = tiles[r][c];
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

                float distance = distancesToTarget[r][c];

                if (drawFlow)
                {
                    if (tile.IsTraversable)
                    {
                        float t = distance / maxDistance;
                        float l = (t * cellSize) / 2f;

                        Vector2 flow = flowField[r][c].normalized * l;
                        Gizmos.DrawRay(pos, flow);
                    }
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

        Build();
        Flow();
        Clear();
    }

    private void Setup()
    {
        tiles = new Tile[numRows][];
        visitedTiles = new bool[numRows][];
        distancesToTarget = new float[numRows][];
        flowField = new Vector2[numRows][];

        for (int r = 0; r < numRows; ++r)
        {
            tiles[r] = new Tile[numCols];
            visitedTiles[r] = new bool[numCols];
            distancesToTarget[r] = new float[numCols];
            flowField[r] = new Vector2[numCols];

            for (int c = 0; c < numCols; ++c)
            {
                tiles[r][c] = new Tile()
                {
                    col = c,
                    row = r,
                    type = TileType.Empty
                };
                visitedTiles[r][c] = false;
                distancesToTarget[r][c] = 0f;
                flowField[r][c] = Vector2.zero;
            }
        }
    }
    private void Build()
    {
        Tile currentTile = GetNearesetTile(target.position);

        Queue<Tile> tilesToVisit = new();
        tilesToVisit.Enqueue(currentTile);
        maxDistance = distancesToTarget[currentTile.row][currentTile.col] = 0f;
        visitedTiles[currentTile.row][currentTile.col] = true;

        while (tilesToVisit.Count > 0)
        {
            currentTile = tilesToVisit.Dequeue();

            float currentDistance = distancesToTarget[currentTile.row][currentTile.col];

            foreach (var offset in neighbours)
            {
                int x = currentTile.col + offset.x;
                int y = currentTile.row + offset.y;

                // Check if out of bounds
                if (IsOutOfBounds(y, x))
                {
                    continue;
                }
                Tile neighbourTile = tiles[y][x];

                // Check if traversable
                if (!neighbourTile.IsTraversable)
                {
                    continue;
                }

                // Check if visited
                if (visitedTiles[y][x])
                {
                    continue;
                }

                float distance = distancesToTarget[y][x] = currentDistance + 1f;
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }

                tilesToVisit.Enqueue(tiles[y][x]);
                visitedTiles[y][x] = true;
            }
        }

        isBuilt = true;
    }
    private void Flow()
    {
        for (int r = 0; r < numRows; ++r)
        {
            for (int c = 0; c < numCols; ++c)
            {
                float currentDistance = distancesToTarget[r][c] + 1;

                float x = GetDistance(r, c - 1, currentDistance) - GetDistance(r, c + 1, currentDistance);
                float y = GetDistance(r + 1, c, currentDistance) - GetDistance(r - 1, c, currentDistance);

                Vector2 flow = new Vector2(x, -y).normalized;
                flowField[r][c] = flow;
            }
        }
    }
    private void Clear()
    {
        for (int r = 0; r < numRows; ++r)
        {
            for (int c = 0; c < numCols; ++c)
            {
                visitedTiles[r][c] = false;
            }
        }
    }

    public bool IsTraversable(int y, int x)
    {
        return tiles[y][x].IsTraversable;
    }
    public bool IsOutOfBounds(int y, int x)
    {
        return (x < 0 || y < 0 || x > numCols - 1 || y > numRows - 1);
    }
    public float GetDistance(int y, int x, float fallback)
    {
        if (IsOutOfBounds(y, x) || !IsTraversable(y, x))
        {
            return fallback;
        }
        return distancesToTarget[y][x];
    }
    public Tile GetNearesetTile(Vector3 position)
    {
        int row = Mathf.Max(Mathf.Min(Mathf.FloorToInt((position.y - offset.y + (cellSize / 2f)) / cellSize), numRows - 1), 0);
        int col = Mathf.Max(Mathf.Min(Mathf.FloorToInt((position.x - offset.x + (cellSize / 2f)) / cellSize), numCols - 1), 0);
        return tiles[row][col];
    }
    public Vector2 GetFlowDir(Vector3 position)
    {
        Tile tile = GetNearesetTile(position);
        return flowField[tile.row][tile.col];
    }
}

public class Tile
{
    public int row;
    public int col;
    public TileType type;

    public bool IsTraversable => type == TileType.Empty;
}

public enum TileType
{
    Empty,
    Obstacle
}