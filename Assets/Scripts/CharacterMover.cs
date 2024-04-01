using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    None,
    Idle,
    Run,
    Burrow,
    ExitBurrow,
}
public class CharacterMover : MonoBehaviour
{
    public MovementState movementState;
    //
    private CharacterInput _input;
    [SerializeField] private float _speed = 2;
    private Vector2 _burrowDirection = Vector2.zero;
    private float _burrowTime = 0;
    //
    [SerializeField] private SpriteRenderer _shadow;
    void Start()
    {
        movementState = MovementState.Idle;
        _input = GetComponent<CharacterInput>();
    }

    //  
    void Update()
    {
        Vector2 direction = new Vector2((_input.HoldingLeft ? -1 : 0) + (_input.HoldingRight ? 1 : 0), (_input.HoldingDowm ? -1 : 0) + (_input.HoldingUp ? 1 : 0)).normalized;
        if (movementState == MovementState.Burrow)
        {
            if (_input.PressedFireRecently)
            {
                // *** STAND STILL  ***
            }
            else
            {
                if (direction.magnitude > 0) _burrowDirection = Vector3.RotateTowards(_burrowDirection, direction, Time.deltaTime * 4, Time.deltaTime);
                transform.position += _speed * 1.7f * Time.deltaTime * _burrowDirection.ToVector3();
            }
            _burrowTime += Time.deltaTime;
            if (_burrowTime > 3f)
            {
                movementState = MovementState.ExitBurrow;
                _burrowTime = 0;
            }
        }
        else if (movementState == MovementState.ExitBurrow)
        {
            _burrowTime += Time.deltaTime;
            if (_burrowTime > 0.3f)
            {
                _shadow.enabled = true;
                if (direction.magnitude > 0)
                {
                    movementState = MovementState.Run;
                }
                else
                {
                    movementState = MovementState.Idle;
                }
            } 
        }
        else if (direction.magnitude > 0)
        {
            movementState = MovementState.Run;
            transform.position += _speed * Time.deltaTime * direction.ToVector3();
        }
        else
        {
            movementState = MovementState.Idle;
        }
        //
        if (_input.PressedFire)
        {
            if (movementState != MovementState.Burrow)
            {
                movementState = MovementState.Burrow;
                _burrowTime = 0;
                _shadow.enabled = false;
                //
                if (direction.magnitude > 0)
                {
                    _burrowDirection = direction;
                }
                else if (_input.LastDirection != 0)
                {
                    _burrowDirection = new Vector2(_input.LastDirection, 0);
                }
                else
                {
                    _burrowDirection = new Vector2(1, 0);
                }
            }
        }
        //
        transform.position = transform.position.WithZ(transform.position.y * 0.02f);
    }
}
