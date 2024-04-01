using FrameworkWIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{

    private bool _up, _left, _right, _down = false;

    private bool _wasUp, _wasLeft, _wasRight, _wasDown = false;

    private bool _wasFire, _fire = false;

    public bool PressedFire => !_wasFire && _fire;

    private float _pressedFireTime = -100;
    public bool PressedFireRecently => Time.time - _pressedFireTime < 0.33f;
    public bool PressedUp => !_wasUp && _up;

    private float _pressedUpTime = -100;
    public bool PressedUpRecently => Time.time - _pressedUpTime < 0.2f;
    public bool HoldingUp => _up;
    public bool HoldingLeft => _left && _forceRightTime <= 0;
    public bool HoldingRight => _right && _forceLeftTime <= 0;
    //
    public int HoldingDirection => (HoldingLeft ? -1 : 0) + (HoldingRight ? 1 : 0);
    public int LastDirection => _lastDirection;
    private int _lastDirection = 0;
    public bool HoldingDowm => _down;
    // 
    //
    private float _forceRightTime = 0f;
    private float _forceLeftTime = 0f;
    //
    private void Start()
    {
       
    }
    //  
    void Update()
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
        else if (!_left &&  _right)
        {
            _lastDirection = 1;
        }
    }
    //
    public bool HasPressedJumpWithin(float seconds) => Time.time - _pressedUpTime < seconds;
    public void ClearJumpInput() => _pressedUpTime = -100;
    //
    public void ForceRightTime(float seconds)
    {
        _forceRightTime = seconds;
    }
    public void ForceLeftTime(float seconds)
    {
        _forceLeftTime = seconds;
    }
}
