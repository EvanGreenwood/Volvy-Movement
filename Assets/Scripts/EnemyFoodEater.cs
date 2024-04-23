using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFoodEater : MonoBehaviour
{
    private CharacterMover _characterMover;
    private Collider2D[] vegetableColliders;
    private int _layerMask =-1;
   

    private void Start()
    {
        _characterMover = GetComponent<CharacterMover>();
        vegetableColliders = new Collider2D[1];
        _layerMask = 1 << LayerMask.NameToLayer("Uprooted Vegetables");
    }
    private void Update()
    {
        if (_characterMover.movementState != MovementState.Stunned && _characterMover.movementState != MovementState.Dead && _characterMover.movementState != MovementState.Eat)
        {
            if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, vegetableColliders, _layerMask) > 0 && vegetableColliders[0].TryGetComponent(out VegetableObject vegetable))
            {
                if (vegetable.CanBeEaten && vegetable.Type != VegetableType.OnionMan)
                {
                    Debug.Log("Found vegetable ");
                    RulesManager.Instance.TryTrigger(RuleTrigger.EnemyEat, UnitManager.Instance.playerTransform.position);
                    _characterMover.Eat(2f);
                    //
                    if (vegetable.Type == VegetableType.RatPoison)
                    {
                        StartCoroutine(DeathRoutine(0.6f));
                    }
                    //
                    Destroy(vegetable.gameObject);
                }
            }
        }
    }
    private IEnumerator DeathRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _characterMover.EnemyHealth.Damage(DamageType.Poison, 10);
    }
}
