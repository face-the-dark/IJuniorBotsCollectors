using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBase : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private ResourcePool _pool;
    [SerializeField] private DeliverHandler _deliverHandler;

    private int _collectedResourcesCount;
    private Queue<Resource> _nonCollectedResources;

    public event Action<int> ScoreChanged;

    private void Awake() => 
        _nonCollectedResources = new Queue<Resource>();

    private void Start() =>
        _collectedResourcesCount = 0;

    private void OnEnable()
    {
        _resourceSpawner.ResourceSpawned += OnResourceSpawned;
        _deliverHandler.UnitDelivered += OnUnitDelivered;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceSpawned -= OnResourceSpawned;
        _deliverHandler.UnitDelivered -= OnUnitDelivered;
    }

    private void OnResourceSpawned(Resource resource) => 
        _nonCollectedResources.Enqueue(resource);

    private void OnUnitDelivered(Unit unit, Resource resource)
    {
        UpdateScore();
        _pool.Release(resource);
        unit.Reset();
    }

    private void UpdateScore()
    {
        _collectedResourcesCount++;
        ScoreChanged?.Invoke(_collectedResourcesCount);

        Debug.Log($"Score: {_collectedResourcesCount}");
    }

    public Resource GetNonCollectedResource()
    {
        if (_nonCollectedResources.Count <= 0)
            return null;

        return _nonCollectedResources.Dequeue();
    }
}