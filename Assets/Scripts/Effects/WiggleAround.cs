using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleAround : MonoBehaviour
{
    private Vector3 _originalPos;
    [SerializeField] private float _wobbleSin1 = 5;
    [SerializeField] private float _wobbleSin2 = 4;
    [SerializeField] private float _wobbleSin3 = 9;
    [SerializeField] private float _wobbleDist = 0.3f;
    void Start()
    {
        _originalPos = transform.localPosition;
    }

    // 
    void Update()
    {
        float sinX = Mathf.Sin(_originalPos.x + Time.time * (_wobbleSin1) + Mathf.Sin(_originalPos.x   + _wobbleSin3 * Time.time)) * _wobbleDist;
        float sinY = Mathf.Sin(_originalPos.x*2 + Time.time * (_wobbleSin2) + Mathf.Sin(_originalPos.x   + _wobbleSin3 * Time.time)) * _wobbleDist;
        transform.localPosition = _originalPos + new Vector3(sinX, sinY, 0);
        transform.localEulerAngles = new Vector3(0, 0, sinX / _wobbleDist * 4);
    }
}
