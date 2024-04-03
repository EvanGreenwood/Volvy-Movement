using FrameworkWIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{

    protected bool _up, _left, _right, _down = false;

    protected bool _wasUp, _wasLeft, _wasRight, _wasDown = false;

    protected bool _wasFire, _fire = false;

    public bool PressedFire => !_wasFire && _fire;

    protected float _pressedFireTime = -100;
    public bool PressedFireRecently => Time.time - _pressedFireTime < 0.33f;
    public bool PressedUp => !_wasUp && _up;

    protected float _pressedUpTime = -100;
    public bool PressedUpRecently => Time.time - _pressedUpTime < 0.2f;
    public bool HoldingUp => _up;
    public bool HoldingLeft => _left && _forceRightTime <= 0;
    public bool HoldingRight => _right && _forceLeftTime <= 0;
    //
    public int HoldingDirection => (HoldingLeft ? -1 : 0) + (HoldingRight ? 1 : 0);
    public int LastDirection => _lastDirection;
    protected int _lastDirection = 0;
    public bool HoldingDowm => _down;
    // 
    //
    protected float _forceRightTime = 0f;
    protected float _forceLeftTime = 0f;
    //
    private void Start()
    {
       
    }
    //  
   protected virtual void Update()
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
