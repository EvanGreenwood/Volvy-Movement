using Framework;
using System;
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
    [SerializeField] private Transform _waterLevelAnchor;
    float _waterLevel = 1;
    [SerializeField] private Transform _exitLevelDummy;
    public List<StomachVegetable> StomachVegetables => _stomachVegetables;
    List<StomachVegetable> _stomachVegetables = new List<StomachVegetable>();
    List<StomachVegetable> _digestingVegetables = new List<StomachVegetable>();
    //
    [Header("Stomach Vegetables")]
    [SerializeField] private int _vegetableMax = 10;
    [SerializeField] private TMPro.TextMeshProUGUI _vegetableCountText;
    [SerializeField] private TMPro.TextMeshProUGUI _vegetableMaxText;
    private int _calculatedVeggieCount = 0;
    //
    [Header("Stomach Walls")]
    [SerializeField] private StomachSphincter _leftSphincter;
    [SerializeField] private StomachSphincter _rightSphincter;
    //
    [SerializeField] private Rigidbody[] _leftSides;
    [SerializeField] private Rigidbody[] _rightSides;
    //
    private Coroutine _digestRoutine;
    //
    [Header("Poop")]
    [SerializeField] private int _poopLevelCount = 0;
    [SerializeField] private int _poopMax = 1;
    [SerializeField] private TMPro.TextMeshProUGUI _poopCountText;
    [SerializeField] private TMPro.TextMeshProUGUI _poopMaxText;
    private int _calculatedPoopCount = 0;

    [Header("Onion Man")]
    [SerializeField] private TMPro.TextMeshProUGUI _onionManGoalText;
    [SerializeField] private TMPro.TextMeshProUGUI _onionManTimerText;
    [SerializeField] private TMPro.TextMeshProUGUI _onionManTimerDescriptionText;
    private float _numberOfOnionManEaten;
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

        if(_waterLevel > 0)
        {
            _waterLevel -= 0.1f * Time.deltaTime;
            _waterLevelAnchor.localScale = Vector3.one.WithY(_waterLevel / 1);
        }
    }
    private IEnumerator DigestRoutine()
    {
        Debug.Log(" Start digesting !! ");
        _digestingVegetables.AddRange(_stomachVegetables);
        _stomachVegetables.Clear();
        _calculatedPoopCount++;
        _poopCountText.text = (_calculatedPoopCount < 10 ? "0" : "") + _calculatedPoopCount.ToString();
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
        EffectsController.Instance.SpawnPoop(UnitManager.Instance.playerTransform.position, 11, 2, _calculatedPoopCount >= _poopMax);
        //
        RulesManager.Instance.TryTrigger(RuleTrigger.VolvyPoop, UnitManager.Instance.playerTransform.position);
        //
        time = 2.2f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            _waterLevelDummy.transform.localPosition = Vector3.Lerp(_waterLevelDummy.transform.localPosition, _waterLevelDummy.transform.localPosition.WithY(-2.6f), Time.deltaTime * 5);
            yield return new WaitForEndOfFrame();
        }
        _digestRoutine = null;
        Debug.Log(" Stop digesting !! ");

        if (_calculatedPoopCount >= _poopMax)
        {
            _calculatedPoopCount = 0;
            if (_poopLevelCount % 2 == 0)
            {
                _poopMax++;
            }
            _poopCountText.text = (_calculatedPoopCount < 10 ? "0" : "") + _calculatedPoopCount.ToString();
            _poopMaxText.text = (_poopMax < 10 ? "0" : "") + _poopMax.ToString();
            //RulesUI.Instance.ActivateRulesUI();
            _poopLevelCount++;
        }
        _waterLevel = 1;
    }
    public void SpawnVegetable()
    {
        StomachVegetable veggie = Instantiate(_vegetablePrefab, _neckTransform.position, Quaternion.identity, transform); 
        _stomachVegetables.Add(veggie);
        _waterLevel = 1;
    }
    public void SpawnVegetable(VegetableType type)
    {
        StomachVegetable veggie = Instantiate(type.stomachPrefab, _neckTransform.position, Quaternion.identity, transform);
        _stomachVegetables.Add(veggie);
        _waterLevel = 1;

        if (type == VegetableType.OnionMan)
            IncreaseOnionManEaten();
    }
    public void SpawnOnion()
    {
        StomachVegetable veggie = Instantiate(_onionPrefab, _neckTransform.position, Quaternion.identity, transform);
        _stomachVegetables.Add(veggie);
        _waterLevel = 1;
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

    public void UpdateOnionManTimer(float currentTime)
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        _onionManTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void WaitingForOnionManToBeEaten()
    {
        _onionManTimerText.text = "Eat Onion Man";
        _onionManTimerDescriptionText.text = "Bump Onion Man 4 times to uproot";
    }

    void IncreaseOnionManEaten()
    {
        _numberOfOnionManEaten++;

        if (_numberOfOnionManEaten >= 4)
        {
            WinLoseScreen.Instance.EndGame(true);
        }
        _onionManGoalText.text = _numberOfOnionManEaten.ToString() + " out of 4 Onion Men Eaten";
    }
}
