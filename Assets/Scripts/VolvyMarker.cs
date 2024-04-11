using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class VolvyMarker : MonoBehaviour
{
     [SerializeField] private AnimatedEffect _effectPrefab;
    [SerializeField] private float _rate = 2;
    private float _counter = 0;
    private CharacterMover _characterMover;
    void Start()
    {
        _characterMover = GetComponent<CharacterMover>();
    }

    // 
    void Update()
    {
        _counter += Time.deltaTime;
        if (_counter >= _rate)
        {
            _counter -= _rate;
            //
            if (UnitManager.Instance.FindNearestEnemy(out Transform targetTransform, transform.position, 9))
            {
                Instantiate(_effectPrefab, targetTransform.position + Vector3.up * 0.87f, Quaternion.identity, targetTransform);
                RulesManager.Instance.TryTrigger(RuleTrigger.MarkEnemy, targetTransform.position);
            }
        }
    }
}
