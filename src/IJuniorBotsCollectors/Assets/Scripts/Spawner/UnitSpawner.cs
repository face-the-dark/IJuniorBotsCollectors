using System;
using System.Collections.Generic;
using UnitComponents;
using UnityEngine;

namespace Spawner
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _resourceBase;
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private int _spawnCount = 3;
        
        public event Action<List<Unit>> UnitsSpawned;

        private void Start()
        {
            List<Unit> spawnedUnits = new List<Unit>();

            for (int i = 0; i < _spawnCount; i++)
            {
                Vector3 position = new Vector3(transform.position.x + i, transform.position.y,
                    transform.position.z + i);
                Unit unit = Instantiate(_unitPrefab, position, Quaternion.identity);
                unit.SetResourceBasePosition(_resourceBase.position);
                spawnedUnits.Add(unit);
            }

            UnitsSpawned?.Invoke(spawnedUnits);
        }
    }
}