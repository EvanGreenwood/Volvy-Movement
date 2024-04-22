using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlowObstacle : MonoBehaviour
{
    [SerializeField] private float scale = 1.25f;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void Start()
    {
        Vector3 size = col.bounds.size * scale;
        Vector2 pos = col.transform.position - size / 2f;

        Tile t1 = FlowFieldManager.Instance.GetNearesetTileAt(pos);
        Tile t2 = FlowFieldManager.Instance.GetNearesetTileAt(new Vector2(pos.x + size.x, pos.y + size.y));

        for (int y = t1.row; y <= t2.row; ++y)
        {
            for (int x = t1.col; x <= t2.col; ++x)
            {
                Tile tile = FlowFieldManager.Instance.GetTile(y, x);
                tile.type = TileType.Obstacle;
                tile.flow = FlowFieldManager.Instance.GetTilePosition(tile) - (Vector2)(transform.position);
                FlowFieldManager.Instance.SetTile(y, x, tile);
            }
        }
    }
}
