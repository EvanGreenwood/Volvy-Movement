using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private float _chargeAmount = 0.5f;
    [SerializeField] private float _chargeRate = 0.2f;
    [SerializeField] private Transform _chargeTransform;
    //  
    void Start()
    {
        
    }

    //  
    void Update()
    {
        if (!UnitManager.Instance.IsVolvyBurrowing)
        {
            _chargeAmount = Mathf.Clamp(_chargeAmount + _chargeRate * Time.deltaTime, 0, 1);
        }
        _chargeTransform.localScale = new Vector3(_chargeAmount, 1, 1);
    }
    public bool TryUseCharge()
    {
        if (_chargeAmount > 0.5f)
        {
            _chargeAmount -= 0.5f;
            return true;
        }
        return false;
    }
    public void AddCharge(float charge)
    {
        _chargeAmount = Mathf.Clamp01(_chargeAmount + charge);
    }
}
