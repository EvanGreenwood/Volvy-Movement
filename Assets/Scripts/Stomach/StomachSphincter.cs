using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachSphincter : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _closed = true;
    [SerializeField] private float _openXOffset = 2;
    private float _closedXPosition = 0;
    public bool IsClosed => _closed;
    void Start()
    {
        _closedXPosition = transform.localPosition.x;
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Open()
    {
        _closed = false;
    }
    public void Close()
    {
        _closed = true;
    }
    // 
    void Update()
    {
        if (_closed)
        {
            _rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, _rigidbody.position.WithX(_closedXPosition), Time.deltaTime * 22));
        } else {
            _rigidbody.MovePosition(Vector3.Lerp(_rigidbody.position, _rigidbody.position.WithX(_closedXPosition + _openXOffset), Time.deltaTime * 12));
        }
    }
}
