using System;
using Base.Scanner;
using ResourceComponents;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    [RequireComponent(typeof(ResourceScanner))]
    public class ResourceBase : MonoBehaviour
    {
        [SerializeField] private UnitProvider _unitProvider;
        [SerializeField] private ResourceSpawner _resourceSpawner;
        [SerializeField] private ResourceDatabase _resourceDatabase;
        [SerializeField] private int _resourcesCountForSpawnNewUnit = 3;

        private int _collectedResourcesCount;
        private bool _hasFlag;
        
        public event Action<int> ScoreChanged;

        public void Construct(ResourceSpawner resourceSpawner, ResourceDatabase resourceDatabase)
        {
            _resourceSpawner = resourceSpawner;
            _resourceDatabase = resourceDatabase;
        }
        
        private void Start()
        {
            _collectedResourcesCount = 0;
            _hasFlag = true;
            _resourceDatabase.AddScanner(GetComponent<ResourceScanner>());
        }

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
            Release(unit, resource);
            TrySpawnNewUnit();
        }

        public void Release(Unit unit, Resource resource)
        {
            unit.Reset();
            IssueNextResourceToUnit(unit);

            _resourceSpawner.Release(resource);
            _resourceDatabase.Release(resource);
        }
        
        public void ChangePriorityToBuild(Vector3 flagPosition) => 
            _unitProvider.SetDeliveryPositionForAllUnits(flagPosition);

        public void ChangePriorityToCollect() => 
            _unitProvider.SetDeliveryPositionForAllUnits(transform.position);

        public bool TryTakeFlag()
        {
            if (_hasFlag)
            {
                _hasFlag = false;
                
                return true;
            }
            
            return false;
        }

        public void ResetFlag() => 
            _hasFlag = true;

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

        public void BuildNewResourceBase(Vector3 flagPosition)
        {
            Unit freeUnit = _unitProvider.GetFreeUnit();

            if (freeUnit)
                freeUnit.MoveToNewResourceBasePosition(flagPosition, _resourceSpawner, _resourceDatabase);
            
            _unitProvider.DisconnectUnit(freeUnit);
        }

        public void JoinUnit(Unit unit)
        {
            _unitProvider.AddUnit(unit);
            unit.Reset();
        }
    }
}