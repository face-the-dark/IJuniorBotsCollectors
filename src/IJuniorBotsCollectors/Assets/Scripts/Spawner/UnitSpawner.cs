using System.Collections.Generic;
using System.Linq;
using UnitComponents;
using UnityEngine;

namespace Spawner
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private int _spawnCount = 3;

        private List<Unit> _spawnedUnits;

        private void Awake() =>
            _spawnedUnits = new List<Unit>();

        private void Start()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                Vector3 position = new Vector3(transform.position.x + i, transform.position.y,
                    transform.position.z + i);
                Unit unit = Instantiate(_unitPrefab, position, Quaternion.identity);
                _spawnedUnits.Add(unit);
            }
        }

        public Unit GetFreeUnit() =>
            _spawnedUnits
                .Select(unit => unit)
                .FirstOrDefault(unit => unit.HasResource == false);
    }
}