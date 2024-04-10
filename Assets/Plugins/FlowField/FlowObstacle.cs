using UnityEngine;

public class FlowObstacle : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private int precision = 4;
    [SerializeField] private float scale = 1.25f;

    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        for (int y = 0; y < precision + 1; ++y)
        {
            float pY = (float)y / precision;
            for (int x = 0; x < precision + 1; ++x)
            {
                float pX = (float)x / precision;

                Vector3 size = col.bounds.size * scale;

                Vector2 pos = col.transform.position - size / 2f;
                pos.x += size.x * pX;
                pos.y += size.y * pY;

                FlowFieldManager.Instance.GetNearesetTile(pos).type = TileType.Obstacle;
            }
        }
    }
}
