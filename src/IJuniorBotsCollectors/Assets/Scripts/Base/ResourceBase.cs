using System;
using System.Collections.Generic;
using ResourceComponents;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    public class ResourceBase : MonoBehaviour
    {
        [SerializeField] private UnitSpawner _unitSpawner;
        [SerializeField] private ResourceSpawner _resourceSpawner;
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

        private void OnResourceSpawned(Resource resource)
        {
            _nonCollectedResources.Enqueue(resource);
            
            IssueNextResourceToUnit(_unitSpawner.GetFreeUnit());
        }

        private void OnUnitDelivered(Unit unit, Resource resource)
        {
            UpdateScore();
            
            unit.Reset();
            IssueNextResourceToUnit(unit);
            
            _resourceSpawner.Release(resource);
        }

        private void IssueNextResourceToUnit(Unit unit)
        {
            if (unit == null) 
                return;
        
            if (TryGetNonCollectedResource(out Resource resource)) 
                unit.AcceptResource(resource);
        }

        private void UpdateScore()
        {
            _collectedResourcesCount++;
            
            ScoreChanged?.Invoke(_collectedResourcesCount);
        }

        private bool TryGetNonCollectedResource(out Resource resource)
        {
            if (_nonCollectedResources.Count <= 0)
            {
                resource = null;
            
                return false;
            }

            resource = _nonCollectedResources.Dequeue();
        
            return true;
        }
    }
}