using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StomachVegetable : MonoBehaviour
{
     
    public int CombineCount => _combineCount;
    [SerializeField] private int _combineCount = 1;
    public VegetableType Type => _vegetableType;
    [SerializeField] private VegetableType _vegetableType;
    public Rigidbody Rigidbody => _rigidbody;
    private Rigidbody _rigidbody;

    //
    private bool _combining = false;
    public bool Ejecting => _ejecting; 
    private bool _ejecting = false;
    private float _ejectingDelay = 0;
    private float _vomitAngle = 0;
    [SerializeField] private float _ejectForce = 60f;
    private Vector3 _originalScale;

    [SerializeField] private bool _autoEject = false;

    private void Awake()
    {
        _originalScale = transform.localScale;
        transform.localScale = _originalScale * 0.02f;
    }
    void Start()
    {
         
        _rigidbody = GetComponent<Rigidbody>();
        // 
        if (_autoEject)
        {
            _ejectingDelay = 0.5f;
            _ejecting = true;
        }
    }

    //  
    void Update()
    {
        transform.localScale =  Vector3.Lerp(transform.localScale, _originalScale, Time.deltaTime * 7)  ;
        if (Input.GetKeyDown(KeyCode.E))
        { 
            _vomitAngle = Random.value *Mathf.PI * 2;
            _ejecting = true;
        }
    }
    private void FixedUpdate()
    {
        if (_ejecting && _ejectingDelay > 0)
        {
            _ejectingDelay -= Time.deltaTime;
        }
        else if (_ejecting)
        {
            _rigidbody.AddForce(Vector3.up * (_rigidbody.velocity.y < 5? _ejectForce  * 2: _ejectForce) * Time.deltaTime, ForceMode.VelocityChange);

            if (transform.position.y >= StomachManager.Instance.NeckTransform.position.y + 1f)
            {
                if (_vegetableType.bombPrefab != null)
                {
                    UnitManager.Instance.VolvyDropBomb(UnitManager.Instance.playerTransform.position, Vector2.zero);
                }
                else if (_vegetableType.seedPrefab != null)
                {
                    Debug.Log("  SpawnShrapnel " + _vomitAngle);
                    Vector2 v = new Vector2(Mathf.Sin(_vomitAngle), Mathf.Cos(_vomitAngle));
                    EffectsController.Instance.SpawnShrapnel(Type, UnitManager.Instance.playerTransform.position, v, 20, 3);
                }
                StomachManager.Instance.RemoveStomachVegetable(this);
                Destroy(gameObject); 
            }
        }
    }
     //
    public void ThrowUpVegetable(float vomitAngle)
    {
        Debug.Log("  ThrowUpVegetable " + vomitAngle);
        _vomitAngle = vomitAngle;
        _ejecting = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_combining && collision.rigidbody != null && collision.rigidbody.TryGetComponent(out StomachVegetable otherVegetable))
        {
            //
            if (RulesManager.Instance.TryCombineVegetables(Type, otherVegetable.Type, out VegetableType result))
            {
                _combining = true;
                //_rigidbody.MovePosition(collision.contacts[0].point);
                StomachManager.Instance.SpawnVegetable(result, collision.contacts[0].point);

                StomachManager.Instance.RemoveStomachVegetable(this);
                StomachManager.Instance.RemoveStomachVegetable(otherVegetable);
                Destroy(gameObject);
                Destroy(otherVegetable.gameObject);
                // 
            }
        }
    }
    public void CalculateScale()
    {
        transform.localScale = Vector3.one * (0.25f + 0.125f * Mathf.Sqrt(CombineCount));
        _rigidbody.mass = 0.75f + 0.5f * Mathf.Sqrt(CombineCount);
    }
    public void HalfScale()
    {
        transform.localScale = _originalScale * 0.33f;
        
    }
    
}
