using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    CharacterMover _mover;
    [SerializeField] int _health = 10;

    bool _canBeDamaged => _invulnerabilityTime <= 0;
    float _invulnerabilityTime;
    private float _deathTime = 0;

    void Start()
    {
        _mover = GetComponent<CharacterMover>();
    }

    void Update()
    {
        if (_mover.movementState == MovementState.Dead)
        {
            // *** DEAD *** 
            if (Time.time - _deathTime > 1.2f)
            {
                Destroy(gameObject);
            }
        }
        else if(_invulnerabilityTime > 0)
        {
            _invulnerabilityTime -= Time.deltaTime;
        }
    }

    public void Damage(DamageType damageType, int damageAmount)
    {
        if (_canBeDamaged)
        {
            _health -= damageAmount;
            EffectsController.Instance.SpawnDamageNumber(damageAmount, transform.position.WithY(transform.position.y + 1.5f));

            if (_health <= 0)
            {
                Die(damageType);
            }
            else
            {
                _mover.Stun(2);
                _invulnerabilityTime = 0.5f;
            }
        }
    }

    void Die(DamageType damageType)
    {
        _mover.movementState = MovementState.Dead;
        if (damageType == DamageType.Poison)
        {
            _mover.Bounce.Bounce(0.0f, 5);
            gameObject.layer = Layer.Default;
            _deathTime = Time.time; 
        }
        else
        {
            EffectsController.Instance.SpawnBoneShrapnel(7, transform.position, 10, 2);
            Destroy(gameObject);
        }

       
    }
}
