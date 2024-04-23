using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class UnitManager : SingletonBehaviour<UnitManager>
{
    public Transform playerTransform;
    public Vector2 VolvyMoveDirection => _volvyMover.MoveDirection;
    public CharacterMover VolvyMover => _volvyMover;
    [SerializeField] CharacterMover _volvyMover;
    [SerializeField] private Bomb _bombPrefab;
    public bool IsVolvyBurrowing => _volvyMover.movementState == MovementState.Burrow || _volvyMover.movementState == MovementState.ExitBurrow;
     
    public List<Transform > _enemies = new List<Transform>();
    //
    public Vector3 GetAvoidanceAmount(Transform enemy)

    {

        Vector3 avoidance = default;
        foreach (Transform t in _enemies)
        {
            if (t != enemy)
            {
                Vector3 diff = (t.position - enemy.position).WithZ(0);
                float dist = diff.magnitude;
                if (dist < 1)
                {
                    avoidance += -diff.normalized *  Mathf.Pow( ( 1 - dist),2) * 0.3f ;
                }
            }
        }

        return avoidance;
    }
    public void VolvyDropBomb(Vector3 position, Vector2 velocity)
    {
        Bomb b=  Instantiate(_bombPrefab, position, Quaternion.identity);
        b.Launch(-_volvyMover.MoveDirection* 2 + velocity * 0.6f, 8);
    }
    //
    public void Explode(Vector3 position, float killRange, float knockRange, float knockForce)
    {
       
        Collider[] enemyColliders = Physics.OverlapSphere(position, Mathf.Max(knockRange, killRange), 1 << LayerMask.NameToLayer("Enemies"));
        //
       // Debug.Log(" Explode " + Mathf.Max(knockRange, killRange) + "  " + enemyColliders.Length);
        //
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            if (enemyColliders[i].TryGetComponent(out CharacterMover mover))
            {
                Vector3 diff = (mover.transform.position - position).WithZ(0);
                if (killRange > 0 && diff.magnitude < killRange)
                {
                    mover.EnemyHealth.Damage(DamageType.Explode, 10);
                }
                else
                {
                    mover.Stun(1f);
                    mover.Knock(diff.normalized * knockForce);
                }
            }
        }
    }
    public bool FindNearestEnemy(out Transform enemyTransform, Vector2 pos, float  maxRange)
    {
        float nearestDist = maxRange;
        int nearestIndex = -1;
        for (int i = 0; i < _enemies.Count; i++)
        {
            float dist = (_enemies[i].transform.position.ToVector2() - pos).magnitude;
            if (dist < nearestDist)
            {
                nearestIndex = i;
                nearestDist = dist;
            }
        }
        if (nearestIndex >= 0)
        {
            enemyTransform = _enemies[nearestIndex];
            return true;
        }
        else
        {
            enemyTransform = null;
            return false;
        }

    }
}
