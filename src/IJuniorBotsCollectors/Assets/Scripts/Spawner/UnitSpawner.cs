using System;
using System.Collections.Generic;
using UnitComponents;
using UnityEngine;

namespace Spawner
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private int _startSpawnCount = 3;

        public int StartSpawnCount => _startSpawnCount;

        public event Action<List<Unit>> UnitsSpawned;

        public void SpawnStartUnits(Vector3[] spawnPositions)
        {
            List<Unit> spawnedUnits = new List<Unit>();

            for (int i = 0; i < _startSpawnCount; i++)
            {
                Unit unit = SpawnUnit(spawnPositions[i]);
                spawnedUnits.Add(unit);
            }

            UnitsSpawned?.Invoke(spawnedUnits);
        }

        public Unit SpawnUnit(Vector3 position)
        {
            Unit unit = Instantiate(_unitPrefab, position, Quaternion.identity);
            unit.SetDeliveryPosition(transform.position);

            return unit;
        }
    }
}