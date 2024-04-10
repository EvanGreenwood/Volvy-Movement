using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachVegetable : MonoBehaviour
{
    public enum VegetableType
    {
        None,
        Onion,
        Vegetable,
    }
    public int CombineCount => _combineCount;
    [SerializeField] private int _combineCount = 1;
    public VegetableType Type => _vegetableType;
    [SerializeField] private VegetableType _vegetableType;
    private Rigidbody _rigidbody;

    [SerializeField] VegetableObject _spawnVegetable;
    //
    private bool _combining = false;
    private bool _ejecting = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateScale();
    }

    //  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Eject"); 
            _ejecting = true;
        }
    }
    private void FixedUpdate()
    {
        if (_ejecting)
        {
            _rigidbody.AddForce(Vector3.up * 40 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    public void ThrowUpVegetable()
    {
        _ejecting = true;

        if(UnitManager.HasInstance && UnitManager.Instance.playerTransform != null)
            Instantiate(_spawnVegetable, UnitManager.Instance.playerTransform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (!_combining && collision.collider.TryGetComponent<StomachVegetable>(out StomachVegetable otherVegetable))
        {
            if (otherVegetable.Type == Type && _combineCount == otherVegetable.CombineCount)
            {
                _combineCount *= 2;
                
                otherVegetable.StartCombining();
                otherVegetable.GetComponentInChildren<Collider>().enabled = false;
                StomachManager.Instance.RemoveStomachVegetable(otherVegetable);
                Destroy(otherVegetable.gameObject);
                //
                _rigidbody.MovePosition(collision.contacts[0].point);
                //
                CalculateScale();
            }
        }*/
    }
    public void CalculateScale()
    {
        transform.localScale = Vector3.one * (0.25f + 0.125f * Mathf.Sqrt(CombineCount));
        _rigidbody.mass = 0.75f + 0.5f * Mathf.Sqrt(CombineCount);
    }
    public void StartCombining()
    {
        _combining = true;
    }
}
