using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class EnemyInput : CharacterInput
{
    public void Move(Vector2 dir)
    {
        if (UnitManager.Instance.playerTransform == null)
            return;

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, dir);
#endif

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) * 2)
        {
            if (dir.x > 0)
            {
                _right = true;
            }
            else if (dir.x < 0)
            {
                _left = true;
            }
        }
        else if (Mathf.Abs(dir.x) * 2 < Mathf.Abs(dir.y))
        {
            if (dir.y > 0)
            {
                _up = true;
            }
            else if (dir.y < 0)
            {
                _down = true;
            }
        }
        else
        {
            if (dir.x > 0)
            {
                _right = true;
            }
            else if (dir.x < 0)
            {
                _left = true;
            }
            if (dir.y > 0)
            {
                _up = true;
            }
            else if (dir.y < 0)
            {
                _down = true;
            }
        }
    }
}
