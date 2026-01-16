using System;
using System.Collections.Generic;
using UnitComponents;
using UnityEngine;

namespace Spawner
{
    public class UnitSpawner : MonoBehaviour
    {
        private const float CircleDegrees = 360f;
        
        [SerializeField] private Transform _resourceBase;
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private int _startSpawnCount = 3;
        [SerializeField] private float _spawnRadius = 2f;

        private List<Unit> _spawnedUnits;
        private int _currentSpawnCount;
        
        public event Action<List<Unit>> UnitsSpawned;

        private void Awake() => 
            _spawnedUnits = new List<Unit>();

        private void Start()
        {
            _currentSpawnCount = _startSpawnCount;

            for (int i = 0; i < _currentSpawnCount; i++)
            {
                Vector3[] spawnPositions = GetSpawnPositions();
                Unit unit = SpawnUnit(spawnPositions[i]);
                _spawnedUnits.Add(unit);
            }

            UnitsSpawned?.Invoke(_spawnedUnits);
        }

        public Unit SpawnNewUnit()
        {
            _currentSpawnCount++;

            Vector3[] spawnPositions = GetSpawnPositions();

            for (var i = 0; i < _spawnedUnits.Count; i++) 
                _spawnedUnits[i].UpdateStartPosition(spawnPositions[i]);

            Unit newUnit = SpawnUnit(spawnPositions[spawnPositions.Length - 1]);
            _spawnedUnits.Add(newUnit);
            
            return newUnit;
        }
        
        private Unit SpawnUnit(Vector3 position)
        {
            Unit unit = Instantiate(_unitPrefab, position, Quaternion.identity);
            unit.SetDeliveryPosition(_resourceBase.position);
            
            return unit;
        }

        private Vector3[] GetSpawnPositions()
        {
            Vector3[] spawnPositions = new Vector3[_currentSpawnCount];
            float angleStep = CircleDegrees / _currentSpawnCount;

            for (int i = 0; i < _currentSpawnCount; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;

                float x = _resourceBase.position.x + Mathf.Cos(angle) * _spawnRadius;
                float z = _resourceBase.position.z + Mathf.Sin(angle) * _spawnRadius;

                spawnPositions[i] = new Vector3(x, 0, z);
            }

            return spawnPositions;
        }
    }
}