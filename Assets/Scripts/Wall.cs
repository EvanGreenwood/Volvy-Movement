using UnityEngine;

public class Wall : MonoBehaviour
{
    private void Start()
    {
        FlowFieldManager.Instance.GetNearesetTile(transform.position).type = TileType.Wall;
    }
}
