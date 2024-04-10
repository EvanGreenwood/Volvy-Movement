using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolvyHealth : MonoBehaviour
{
    CharacterMover _mover;

    [SerializeField] float _collisionRadius = 0.5f;
    bool _isInvulnerable => _invulnerabilityTime > 0;
    float _invulnerabilityTime;
    void Start()
    {
        _mover = GetComponent<CharacterMover>();
    }
    void Update()
    {
        if (_isInvulnerable)
            _invulnerabilityTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        CheckEnemyCollisions();
    }

    void CheckEnemyCollisions()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, _collisionRadius, 1 << Layer.Enemies);

        foreach (Collider c in enemyColliders)
        {
            if (!UnitManager.Instance.IsVolvyBurrowing)
            {
                if (c.TryGetComponent(out CharacterMover enemyMover))
                {
                    if (enemyMover.movementState != MovementState.Dead && enemyMover.movementState != MovementState.Stunned)
                    {
                        Vector3 dir = (c.transform.position - transform.position).WithZ(0);

                        enemyMover.Knock(dir.normalized * 20);
                        //
                        if (!_isInvulnerable)
                        {
                            if (StomachManager.HasInstance && StomachManager.Instance.StomachVegetables.Count > 0)
                                TakeDamage();
                            else
                                Die();
                        }
                    }
                }
            }
        }
    }

    void TakeDamage()
    {
        StomachManager.Instance.ThrowUpVegetables();
        _invulnerabilityTime = 1f;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
