using System;
using Base.Scanner;
using ResourceComponents;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    [RequireComponent(typeof(UnitProvider))]
    [RequireComponent(typeof(ResourceScanner))]
    public class ResourceBase : MonoBehaviour
    {
        [SerializeField] private int _resourcesCountForSpawnNewUnit = 3;
        [SerializeField] private int _resourcesCountForBuildNewResourceBase = 5;
        [SerializeField] private Flag _flagPrefab;

        private UnitProvider _unitProvider;
        private ResourceBaseBuilder _resourceBaseBuilder;
        private ResourceSpawner _resourceSpawner;
        private ResourceDatabase _resourceDatabase;

        private int _collectedResourcesCount;
        private Flag _flag;
        private TargetPriority _targetPriority;

        public event Action<int> ScoreChanged;

        public void Construct
        (
            ResourceSpawner resourceSpawner,
            ResourceDatabase resourceDatabase,
            ResourceBaseBuilder resourceBaseBuilder
        )
        {
            _resourceSpawner = resourceSpawner;
            _resourceDatabase = resourceDatabase;
            _resourceBaseBuilder = resourceBaseBuilder;

            _resourceBaseBuilder.Built += OnBuilt;
        }

        private void Awake() =>
            _unitProvider = GetComponent<UnitProvider>();

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

        public void SpawnStartUnits() =>
            _unitProvider.SpawnStartUnits();

        public void PickUpResource(Unit unit, Resource resource)
        {
            UpdateScore();
            Release(unit, resource);
            TryRunTargetPolicy();
        }

        public Flag TakeFlag()
        {
            _flag.gameObject.SetActive(true);

            return _flag;
        }

        public void ResetFlag()
        {
            _flag.gameObject.SetActive(false);
            _flag.transform.position = transform.position;
        }

        public void ChangePriority(TargetPriority targetPriority)
        {
            _targetPriority = targetPriority;
        }

        public void JoinUnit(Unit unit)
        {
            _unitProvider.ConnectUnit(unit);
            
            unit.SetDeliveryPosition(transform.position);
            unit.Reset();
        }

        private void OnBuilt()
        {
            _resourceBaseBuilder.Built -= OnBuilt;

            _resourceDatabase.AddScanner(GetComponent<ResourceScanner>());

            InitFlag();
        }

        private void InitFlag()
        {
            _flag = Instantiate(_flagPrefab, transform.position, Quaternion.identity);
            _flag.gameObject.SetActive(false);
        }

        private void TryRunTargetPolicy()
        {
            switch (_targetPriority)
            {
                case TargetPriority.SpawnNewUnits:
                    TrySpawnNewUnit();
                    break;
                
                case TargetPriority.BuildNewResourceBase:
                    TryBuildNewResourceBase();
                    break;
            }
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

        private void TryBuildNewResourceBase()
        {
            Unit freeUnit = _unitProvider.GetFreeUnit();
            
            if (freeUnit)
            {
                if (_collectedResourcesCount >= _resourcesCountForBuildNewResourceBase)
                {
                    _collectedResourcesCount -= _resourcesCountForBuildNewResourceBase;

                    freeUnit.BuildNewResourceBasePosition(_flag.transform.position, _resourceBaseBuilder);
                    _unitProvider.DisconnectUnit(freeUnit);
                    
                    ChangePriority(TargetPriority.SpawnNewUnits);
                }
            }
        }

        private void UpdateScore()
        {
            _collectedResourcesCount++;

            ScoreChanged?.Invoke(_collectedResourcesCount);
        }

        private void Release(Unit unit, Resource resource)
        {
            unit.Reset();
            IssueNextResourceToUnit(unit);

            _resourceSpawner.Release(resource);
            _resourceDatabase.Release(resource);
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