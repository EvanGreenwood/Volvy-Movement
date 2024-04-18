using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachManager : SingletonBehaviour<StomachManager>
{
    [SerializeField] private StomachVegetable _vegetablePrefab;
    [SerializeField] private StomachVegetable _onionPrefab;
    public Transform NeckTransform => _neckTransform;
    [SerializeField] private Transform _neckTransform;
    [SerializeField] private Transform _waterLevelDummy;
    [SerializeField] private Transform _exitLevelDummy;
    public List<StomachVegetable> StomachVegetables => _stomachVegetables;
    List<StomachVegetable> _stomachVegetables = new List<StomachVegetable>();
    List<StomachVegetable> _digestingVegetables = new List<StomachVegetable>();
    //
    [SerializeField] private int _vegetableMax = 10;
    [SerializeField] private TMPro.TextMeshProUGUI _vegetableCountText;
    [SerializeField] private TMPro.TextMeshProUGUI _vegetableMaxText;
    //
    [SerializeField] private StomachSphincter _leftSphincter;
    [SerializeField] private StomachSphincter _rightSphincter;
    //
    [SerializeField] private Rigidbody[] _leftSides;
    [SerializeField] private Rigidbody[] _rightSides;
    //
    private Coroutine _digestRoutine;
    //
    private int _calculatedVeggieCount = 0;
    //
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnVegetable();
        }//
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnOnion();
        }//
         //
        _calculatedVeggieCount = 0;
        for (int i = _stomachVegetables.Count - 1; i >= 0; i--)
        {
            if (_stomachVegetables[i] == null)
            {
                Debug.LogError(" Null vegetable");
                _stomachVegetables.RemoveAt(i);
            }
        }
        foreach (StomachVegetable veg in _stomachVegetables)
        {
            if (veg.transform.position.y < _waterLevelDummy.transform.position.y + 0.2f && ! veg.Ejecting)
            {
                _calculatedVeggieCount++;
                if (veg.Rigidbody.velocity.y < -2) veg.Rigidbody.AddForce(Vector3.up * 40 * Time.deltaTime, ForceMode.VelocityChange);
                veg.Rigidbody.velocity *= 1 - Time.deltaTime * 12;
                veg.Rigidbody.angularVelocity *= 1 - Time.deltaTime * 12;
            }
        }
        for (int i = _digestingVegetables.Count - 1; i >= 0; i--)
        {
            if (_digestingVegetables[i] == null)
            {
                Debug.LogError(" Null vegetable");
                _digestingVegetables.RemoveAt(i);
            }
        }
        foreach (StomachVegetable veg in _digestingVegetables)
        {
            if (veg.transform.position.y < _waterLevelDummy.transform.position.y && ! veg.Ejecting)
            {
                _calculatedVeggieCount++; 
            }
        }
        _vegetableCountText.text = (_calculatedVeggieCount < 10 ? "0" : "") + _calculatedVeggieCount.ToString();
        _vegetableMaxText.text = (_vegetableMax < 10 ? "0" : "") + _vegetableMax.ToString();
        //
        if (_calculatedVeggieCount >= _vegetableMax && _leftSphincter.IsClosed)
        {
            if (_digestRoutine == null)
            {
                _digestRoutine = StartCoroutine(DigestRoutine());
            }
        }
        if (_digestRoutine == null) _waterLevelDummy.transform.localPosition = Vector3.Lerp(_waterLevelDummy.transform.localPosition, _waterLevelDummy.transform.localPosition.WithY(-2.6f), Time.deltaTime * 5);
        //
        for (int i = _stomachVegetables.Count - 1; i >= 0; i--)
        {
            if (_stomachVegetables[i].transform.position.y < _exitLevelDummy.transform.position.y)
            {
                Destroy(_stomachVegetables[i].gameObject);
                _stomachVegetables.RemoveAt(i);
            }
        }
        for (int i = _digestingVegetables.Count - 1; i >= 0; i--)
        {
            if (_digestingVegetables[i].transform.position.y < _exitLevelDummy.transform.position.y)
            {
                Destroy(_digestingVegetables[i].gameObject);
                _digestingVegetables.RemoveAt(i);
            }
        }
    }
    private IEnumerator DigestRoutine()
    {
        Debug.Log(" Start digesting !! ");
        _digestingVegetables.AddRange(_stomachVegetables);
        _stomachVegetables.Clear();
        //
        yield return null;
        _leftSphincter.Open();
        _rightSphincter.Open();
        //
        float time = 1;
        while (time > 0)
        {
            time -= Time.deltaTime;
            //
            _waterLevelDummy.transform.localPosition = Vector3.Lerp(_waterLevelDummy.transform.localPosition, _waterLevelDummy.transform.localPosition.WithY(  - 5.5f), Time.deltaTime * 1.4f);
            //
            if (time > 0.6f)
            {
                foreach (Rigidbody side in _leftSides) side.AddForce((Vector3.right + Vector3.up) * 4 * Time.deltaTime, ForceMode.VelocityChange);
                foreach (Rigidbody side in _rightSides) side.AddForce((Vector3.left + Vector3.up) * 4 * Time.deltaTime, ForceMode.VelocityChange);
            }

            yield return new WaitForEndOfFrame();
        } 
        time = 0.5f;
        while (time > 0 && _calculatedVeggieCount > 0)
        {
            time -= Time.deltaTime;
            _waterLevelDummy.transform.localPosition = Vector3.Lerp(_waterLevelDummy.transform.localPosition, _waterLevelDummy.transform.localPosition.WithY(-5.5f), Time.deltaTime * 1.4f);

            yield return new WaitForEndOfFrame();
        }
        for (int i = _digestingVegetables.Count - 1; i >= 0; i--)
        {
            Destroy(_digestingVegetables[i].gameObject);
            _digestingVegetables.RemoveAt(i);
        }
        _digestingVegetables.Clear();
        //
       
            //
            _leftSphincter.Close();
        _rightSphincter.Close();
        //
        time = 0.8f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            _waterLevelDummy.transform.localPosition = Vector3.Lerp(_waterLevelDummy.transform.localPosition, _waterLevelDummy.transform.localPosition.WithY(-2.6f), Time.deltaTime * 5);
            yield return new WaitForEndOfFrame();
        }
        _digestRoutine = null;
        Debug.Log(" Stop digesting !! ");
    }
    public void SpawnVegetable()
    {
        StomachVegetable veggie = Instantiate(_vegetablePrefab, _neckTransform.position, Quaternion.identity, transform); 
        _stomachVegetables.Add(veggie); 
    }
    public void SpawnVegetable(VegetableType type)
    {
        StomachVegetable veggie = Instantiate(type.stomachPrefab, _neckTransform.position, Quaternion.identity, transform);
        _stomachVegetables.Add(veggie);
    }
    public void SpawnOnion()
    {
        StomachVegetable veggie = Instantiate(_onionPrefab, _neckTransform.position, Quaternion.identity, transform);
        _stomachVegetables.Add(veggie);
    }
    public void SpawnVegetable(VegetableType type, Vector3 pos)
    {
        StomachVegetable veggie = Instantiate(type.stomachPrefab, pos.WithZ(_neckTransform.position.z), Quaternion.identity, transform);
        veggie.HalfScale();
    }
    public void ThrowUpVegetables()
    {
        for (int i = 0; i < _stomachVegetables.Count; i++)
        {
            Debug.Log(" Throw up " + ((Mathf.PI * 2f / _stomachVegetables.Count) * i));
            _stomachVegetables[i].ThrowUpVegetable(  Time.time * 0.2f + (Mathf.PI * 2f / _stomachVegetables.Count) * i);
        } 
        _stomachVegetables.Clear();
    }

    public void RemoveStomachVegetable(StomachVegetable stomachVegetable)
    {
        _stomachVegetables.Remove(stomachVegetable);
        _digestingVegetables.Remove(stomachVegetable);
    }
}
