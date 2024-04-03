using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class EnemyInput : CharacterInput
{

    protected override void Update()
    {
        base.Update();
        //
        Vector3 playerPos = UnitManager.Instance.playerTransform.position;
        Vector3 diff = (playerPos - transform.position).WithZ(0)  ;

        //
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y) * 2)
        {
            if (diff.x > 0)
            {
                _right = true;
            }
            else if (diff.x < 0)
            {
                _left = true;
            }
        }
        else if (Mathf.Abs(diff.x) * 2 < Mathf.Abs(diff.y))
        {
            if (diff.y > 0)
            {
                _up = true;
            }
            else if (diff.y < 0)
            {
                _down = true;
            }
        }
        else
        { 
            if (diff.x > 0)
            {
                _right = true;
            }
            else if (diff.x < 0)
            {
                _left = true;
            }
            if (diff.y > 0)
            {
                _up = true;
            }
            else if (diff.y < 0)
            {
                _down = true;
            }
        }
    }
}
