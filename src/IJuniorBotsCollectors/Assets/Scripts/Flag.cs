using System;
using Base;
using ResourceComponents;
using UnitComponents;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private int _countResourcesForBuildNewResourceBase = 5;
    
    private ResourceBase _resourceBase;
    
    private int _collectedResourcesCount;

    public event Action<int> CollectedResourcesCountChanged;

    public void Construct(ResourceBase resourceBase)
    {
        _resourceBase = resourceBase;
    }

    private void Start() => 
        _collectedResourcesCount = 0;

    public void PickUpResource(Unit unit, Resource currentResource)
    {
        _resourceBase.Release(unit, currentResource);
        _collectedResourcesCount++;
        
        TryBuildNewResourceBase();

        CollectedResourcesCountChanged?.Invoke(_collectedResourcesCount);
    }

    private void TryBuildNewResourceBase()
    {
        if (_collectedResourcesCount == _countResourcesForBuildNewResourceBase)
        {
            _resourceBase.BuildNewResourceBase(transform.position);
        }
    }
}