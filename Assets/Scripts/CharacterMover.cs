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
    Stunned,
    Dead,
}
public class CharacterMover : MonoBehaviour
{
    public MovementState movementState;
    //
    private CharacterInput _input;
    [SerializeField] private float _speed = 2;
    private Vector2 _burrowDirection = Vector2.zero;
    private float _burrowTime = 0;
    private float _deathTime = 0;
    //
    [SerializeField] private SpriteRenderer _shadow;
    [SerializeField] private bool _avoidEnemies = false;
    private BounceTransform _bounce;
    private float _stunTime = 0;
    private Vector3 _knockForce = Vector3.zero;
    public Vector2 MoveDirection => _moveDirection;
    private Vector2 _moveDirection = Vector2.zero;
    void Start()
    {
        movementState = MovementState.Idle;
        _input = GetComponent<CharacterInput>();
        _bounce= GetComponentInChildren<BounceTransform>();
        //
        if (_avoidEnemies) UnitManager.Instance._enemies.Add(transform);
    }
    private void OnDestroy()
    {
        if (_avoidEnemies && UnitManager.HasInstance) UnitManager.Instance._enemies.Remove(transform);
    }
    //  
    void Update()
    {
        Vector2 direction = new Vector2((_input.HoldingLeft ? -1 : 0) + (_input.HoldingRight ? 1 : 0), (_input.HoldingDowm ? -1 : 0) + (_input.HoldingUp ? 1 : 0)).normalized;
        _moveDirection = direction;
        //
        if (movementState == MovementState.Dead)
        {
            // *** DEAD *** 
            if (Time.time - _deathTime > 1.2f)
            {
                Destroy(gameObject);
            }
        }
        else if (movementState == MovementState.Stunned)
        {
            // *** NOTHING ***
            _stunTime -= Time.deltaTime;
            if (_stunTime <= 0) movementState = MovementState.Idle;
        } 
        else  if (movementState == MovementState.Burrow)
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
                //
                EffectsController.Instance.SpawnShrapnel(5, transform.position, 15, 2.5f);
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
                if (AbilitiesManager.Instance.chargeBar.TryUseCharge())
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
        }
        //
        if (_avoidEnemies && movementState != MovementState.Stunned && movementState != MovementState.Dead)
        {
            transform.position += UnitManager.Instance.GetAvoidanceAmount(transform) + _knockForce * Time.deltaTime;
        }
        else if (movementState == MovementState.Stunned)
        {
            transform.position +=  _knockForce * Time.deltaTime;
        }
        _knockForce *= 1 - Time.deltaTime * 6;
        //
        transform.position = transform.position.WithZ(transform.position.y * 0.02f);
    }
    public void Stun(float time)
    {
        _bounce.Bounce(0.5f, 5);
        movementState = MovementState.Stunned;
        _stunTime = time;
    }
    public void Damage(float damageAmount)
    {
        _bounce.Bounce(0.0f, 5);
        movementState = MovementState.Dead;
        gameObject.layer = Layer.Default;
        _deathTime = Time.time;
    }
    public void Knock(Vector3 velocity)
    {
        _knockForce = velocity;
    }
}
