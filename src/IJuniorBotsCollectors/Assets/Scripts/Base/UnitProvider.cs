using System.Collections.Generic;
using System.Linq;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    [RequireComponent(typeof(UnitSpawner))]
    [RequireComponent(typeof(PositionCalculator))]
    public class UnitProvider : MonoBehaviour
    {
        private UnitSpawner _unitSpawner;
        private PositionCalculator  _positionCalculator;

        private List<Unit> _units;
        private int _currentUnitsCount;
        
        private void Awake()
        {
            _unitSpawner = GetComponent<UnitSpawner>();
            _positionCalculator = GetComponent<PositionCalculator>();
            
            _units = new List<Unit>();
            _currentUnitsCount = 0;
        }

        private void OnEnable() =>
            _unitSpawner.UnitsSpawned += InitUnits;

        private void OnDisable() =>
            _unitSpawner.UnitsSpawned -= InitUnits;

        public Unit GetFreeUnit() =>
            _units.FirstOrDefault(unit => unit.HasResource == false);

        public void SpawnStartUnits()
        {
            _currentUnitsCount = _unitSpawner.StartSpawnCount;
            
            Vector3[] spawnPositions = _positionCalculator.CalculateSpawnPositions(_currentUnitsCount);
            _unitSpawner.SpawnStartUnits(spawnPositions);
        }

        public void CreateNewUnit()
        {
            _currentUnitsCount++;
            
            Vector3[] spawnPositions = _positionCalculator.CalculateSpawnPositions(_currentUnitsCount);

            UpdateStartPositionsForAllUnits(spawnPositions);
            
            _units.Add(_unitSpawner.SpawnUnit(spawnPositions[^1]));
        }

        public void DisconnectUnit(Unit unit)
        {
            _currentUnitsCount--;
            
            Vector3[] spawnPositions = _positionCalculator.CalculateSpawnPositions(_currentUnitsCount);

            _units.Remove(unit);
            
            UpdateStartPositionsForAllUnits(spawnPositions);
        }

        public void ConnectUnit(Unit unit)
        {
            _currentUnitsCount++;
            
            Vector3[] spawnPositions = _positionCalculator.CalculateSpawnPositions(_currentUnitsCount);

            _units.Add(unit);
            
            UpdateStartPositionsForAllUnits(spawnPositions);
        }

        private void InitUnits(List<Unit> units) =>
            _units.AddRange(units);

        private void UpdateStartPositionsForAllUnits(Vector3[] spawnPositions)
        {
            for (int i = 0; i < _units.Count; i++)
                _units[i].UpdateStartPosition(spawnPositions[i]);
        }
    }
}