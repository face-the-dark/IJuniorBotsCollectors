using System;
using ResourceComponents;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    public class ResourceBase : MonoBehaviour
    {
        [SerializeField] private UnitProvider _unitProvider;
        [SerializeField] private ResourceSpawner _resourceSpawner;
        [SerializeField] private ResourceDatabase _resourceDatabase;
        [SerializeField] private int _resourcesCountForSpawnNewUnit = 3;

        private int _collectedResourcesCount;

        public event Action<int> ScoreChanged;

        private void Start() =>
            _collectedResourcesCount = 0;

        private void Update()
        {
            if (_resourceDatabase.FoundResourcesCount > 0)
            {
                Unit freeUnit = _unitProvider.GetFreeUnit();

                if (freeUnit)
                    IssueNextResourceToUnit(freeUnit);
            }
        }

        public void PickUpResource(Unit unit, Resource resource)
        {
            UpdateScore();
            
            unit.Reset();
            IssueNextResourceToUnit(unit);

            _resourceSpawner.Release(resource);
            _resourceDatabase.Release(resource);

            TrySpawnNewUnit();
        }

        private void TrySpawnNewUnit()
        {
            if (_collectedResourcesCount == _resourcesCountForSpawnNewUnit)
            {
                _unitProvider.CreateNewUnit();
                _collectedResourcesCount -= _resourcesCountForSpawnNewUnit;
                
                ScoreChanged?.Invoke(_collectedResourcesCount);
            }
        }

        private void UpdateScore()
        {
            _collectedResourcesCount++;

            ScoreChanged?.Invoke(_collectedResourcesCount);
        }

        private void IssueNextResourceToUnit(Unit unit)
        {
            if (unit == null)
                return;

            Resource freeResource = _resourceDatabase.GetFreeResource();

            if (freeResource)
                unit.AcceptResource(freeResource);
        }
    }
}