using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFoodEater : MonoBehaviour
{
    private CharacterMover _characterMover;
    private Collider[] vegetableColliders;
    private int _layerMask =-1;
   

    private void Start()
    {
        _characterMover = GetComponent<CharacterMover>();
        vegetableColliders = new Collider[1];
        _layerMask = 1 << LayerMask.NameToLayer("Uprooted Vegetables");
    }
    private void Update()
    {
         
        if (Physics.OverlapSphereNonAlloc(transform.position, 0.5f, vegetableColliders, _layerMask) > 0 && vegetableColliders[0].TryGetComponent(out VegetableObject vegetable))
        {
            Debug.Log("Found vegetable ");
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
    private IEnumerator DeathRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _characterMover.Damage(DamageType.Poison, 2);
    }
}
