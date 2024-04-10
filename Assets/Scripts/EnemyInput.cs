using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class EnemyInput : CharacterInput
{
    protected override void Update()
    {
        //base.Update(); *** DON'T USE BASE
    }
    public void Move(Vector2 dir)
    {
        _wasUp = _up;
        _wasLeft = _left;
        _wasRight = _right;
        _wasDown = _down;
        _wasFire = _fire;
        //
        _up = false;
        _left = false;
        _right = false;
        _down = false;
        _fire = false;
        //
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
