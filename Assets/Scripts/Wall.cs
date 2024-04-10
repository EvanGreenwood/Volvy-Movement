using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Wall : MonoBehaviour
{
    private void Start()
    {
        FlowFieldManager.Instance.GetNearesetTile(transform.position).isTraversable = false;
    }
}
