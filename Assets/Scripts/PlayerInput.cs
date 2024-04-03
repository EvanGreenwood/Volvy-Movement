using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterInput
{
    // 
    void Start()
    {
        
    }

    // 
    protected override void Update()
    {
        base.Update();
        //
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            _up = true;
            if (!_wasUp) _pressedUpTime = Time.time;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            _down = true;
        }
        if (_forceRightTime > 0)
        {
            _forceRightTime -= Time.deltaTime;
            _right = true;
        }
        else if (_forceLeftTime > 0)
        {
            _forceLeftTime -= Time.deltaTime;
            _left = true;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _left = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _right = true;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _fire = true;
            if (!_wasFire)
            {
                _pressedFireTime = Time.time;
            }
        }
        //

        // 
        //
        if (_left && !_right)
        {
            _lastDirection = -1;
        }
        else if (!_left && _right)
        {
            _lastDirection = 1;
        }
    }
}
