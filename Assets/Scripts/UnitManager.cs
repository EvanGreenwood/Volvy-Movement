using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class UnitManager : SingletonBehaviour<UnitManager>
{
    public Transform playerTransform;
    [SerializeField] CharacterMover _volvyMover;
    public bool IsVolvyBurrowing => _volvyMover.movementState == MovementState.Burrow || _volvyMover.movementState == MovementState.ExitBurrow;
     
    public List<Transform > _enemies = new List<Transform>();
    //
    public Vector3 GetAvoidanceAmount(Transform enemy)

    {

        Vector3 avoidance = default;
        foreach (Transform t in _enemies)
        {
            if (t != enemy)
            {
                Vector3 diff = (t.position - enemy.position).WithZ(0);
                float dist = diff.magnitude;
                if (dist < 1)
                {
                    avoidance += -diff.normalized *  Mathf.Pow( ( 1 - dist),2) * 0.3f ;
                }
            }
        }

        return avoidance;
    }
}
