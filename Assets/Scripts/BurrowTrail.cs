using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowTrail : MonoBehaviour
{
    private CharacterMover _mover;
    private List<BurrowTrailObject> _trailObjects = new List<BurrowTrailObject>();
    [SerializeField] private BurrowTrailObject _burrowObjectPrefab;
    [SerializeField] private float _trailDistance = 1f;
    [SerializeField] private float _trailRemoveDistance = 2f;
    private float _trailRemoveTime = -10;
    private int _trailCount = 0;
    void Start()
    {
        _mover = GetComponent<CharacterMover>();    
    }

    // 
    void Update()
    {
        if (_mover.movementState == MovementState.Burrow)
        {
            if (_trailObjects.Count == 0)
            {
                AddTrailObject();
                //
               // EffectsController.Instance.SpawnShrapnel(1, transform.position, 11, 2);
            }
            else
            {
                if ((_trailObjects[_trailObjects.Count - 1].transform.position - transform.position).magnitude > _trailDistance)
                {
                    AddTrailObject();
                    _trailCount++;
                    if (_trailCount % 3 == 0)
                    {
                       // EffectsController.Instance.SpawnShrapnel(1, transform.position, 11, 2);
                    }
                }
                //
                TryRemoveBurrows();
            }
            //
            Collider[] vegetableColliders = Physics.OverlapSphere(transform.position, 0.6f, 1 << LayerMask.NameToLayer("Rooted Vegetables")); 
            for (int i = 0; i < vegetableColliders.Length; i++)
            {
                if (vegetableColliders[i].TryGetComponent<VegetableObject>(out VegetableObject vegetable))
                {
                    vegetable.Uproot();
                }
            }
            //
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 0.6f, 1 << LayerMask.NameToLayer("Enemies"));
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i].TryGetComponent<CharacterMover>(out CharacterMover mover))
                {
                    // mover.Damage(1);
                    if (mover.movementState != MovementState.Stunned && mover.movementState != MovementState.Dead)
                    {
                        RulesManager.Instance.TryTrigger(RuleTrigger.BumpEnemy, transform.position);
                    }
                    mover.EnemyHealth.Damage(DamageType.Bump, 1);
                }
            }
        }
        else
        {
            TryRemoveBurrows();
        }
    }

    private void TryRemoveBurrows()
    {
        if (Time.time - _trailRemoveTime > 0.05f)
        {

            for (int i = 0; i < _trailObjects.Count; i++)
            {
                if (_mover.movementState != MovementState.Burrow || (_trailObjects[i].transform.position - transform.position).magnitude > _trailRemoveDistance)
                {
                    _trailRemoveTime = Time.time;
                    _trailObjects[i].StartShrinking();
                    _trailObjects.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void AddTrailObject()
    {
        _trailObjects.Add(Instantiate(_burrowObjectPrefab, transform.position  + Random.onUnitSphere.WithZ(0) * 0.15f, Quaternion.identity));
        _trailObjects[_trailObjects.Count -1]. transform.position = _trailObjects[_trailObjects.Count - 1].transform.position.WithZ(_trailObjects[_trailObjects.Count - 1].transform.position.y * 0.02f);
    }
}
