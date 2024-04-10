using System.Collections.Generic;
using UnityEngine;

public class FlowObstacle : MonoBehaviour
{
    [SerializeField] private float scale = 1.25f;
    //[SerializeField] private int precision = 4;

    private List<Tile> tiles = new();
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void Start()
    {
        Vector3 size = col.bounds.size * scale;
        Vector2 pos = col.transform.position - size / 2f;

        Tile t1 = FlowFieldManager.Instance.GetNearesetTile(pos);
        Tile t2 = FlowFieldManager.Instance.GetNearesetTile(new Vector2(pos.x + size.x, pos.y + size.y));

        for (int y = t1.row; y <= t2.row; ++y)
        {
            for (int x = t1.col; x <= t2.col; ++x)
            {
                Tile tile = FlowFieldManager.Instance.GetTile(y, x);
                tile.type = TileType.Obstacle;

                tile.flow = FlowFieldManager.Instance.GetTilePosition(tile) - (Vector2)(transform.position);
            }
        }

        //for (int y = 0; y < precision + 1; ++y)
        //{
        //    float pY = (float)y / precision;
        //    for (int x = 0; x < precision + 1; ++x)
        //    {
        //        float pX = (float)x / precision;

        //        Vector3 size = col.bounds.size * scale;

        //        Vector2 pos = col.transform.position - size / 2f;
        //        pos.x += size.x * pX;
        //        pos.y += size.y * pY;

        //        Tile tile = FlowFieldManager.Instance.GetNearesetTile(pos);
        //        tiles.Add(tile);

        //        tile.type = TileType.Obstacle;
        //        tile.flow = pos - (Vector2)(transform.position);
        //    }
        //}
    }

    private void OnEnable()
    {
        foreach (Tile tile in tiles)
        {
            tile.type = TileType.Obstacle;
        }
    }
    private void OnDisable()
    {
        foreach (Tile tile in tiles)
        {
            tile.type = TileType.Empty;
        }
    }
}
