using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachManager : SingletonBehaviour<StomachManager>
{
    [SerializeField] private StomachVegetable _vegetablePrefab;
    [SerializeField] private Transform _neckTransform;
    public List<StomachVegetable> StomachVegetables => _stomachVegetables;
    List<StomachVegetable> _stomachVegetables = new List<StomachVegetable>();
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnVegetable();
        }
    }

    public void SpawnVegetable()
    {
        StomachVegetable veggie = Instantiate(_vegetablePrefab, _neckTransform.position, Quaternion.identity, transform);
        veggie.transform.localScale = Vector3.one * (0.5f + Random.value * 0.25f);
        _stomachVegetables.Add(veggie);
    }

    public void ThrowUpVegetables()
    {
        foreach(StomachVegetable stomachVegetable in _stomachVegetables)
        {
            stomachVegetable.ThrowUpVegetable();
        }
        _stomachVegetables.Clear();
    }

    public void RemoveStomachVegetable(StomachVegetable stomachVegetable)
    {
        _stomachVegetables.Remove(stomachVegetable);
    }
}
