using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTransform : MonoBehaviour
{
    private float _ySpeed =  0;
    private float _yOffset = 0;
    [SerializeField] private float _gravity = -10;
     

    // 
    void Update()
    {
        if (_ySpeed > 0 || _yOffset >0)
        {
            _ySpeed += _gravity * Time.deltaTime;
            _yOffset += _ySpeed * Time.deltaTime;
            if (_yOffset <= 0) { _yOffset = 0; }
            transform.localPosition = new Vector3(0, _yOffset, 0);
        }
    }
    public void Bounce(float offset, float speed)
    {
        if (_yOffset <= 0)
        {
            _yOffset += offset;
            _ySpeed = speed;
        }
    }
}
