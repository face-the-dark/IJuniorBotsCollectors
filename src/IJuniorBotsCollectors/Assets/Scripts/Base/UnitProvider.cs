using System.Collections.Generic;
using System.Linq;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    public class UnitProvider : MonoBehaviour
    {
        [SerializeField] private UnitSpawner _unitSpawner;

        private List<Unit> _units;

        private void Awake() => 
            _units = new List<Unit>();

        private void OnEnable() =>
            _unitSpawner.UnitsSpawned += InitUnits;

        private void OnDisable() =>
            _unitSpawner.UnitsSpawned -= InitUnits;

        private void InitUnits(List<Unit> units) =>
            _units.AddRange(units);

        public Unit GetFreeUnit() =>
            _units.FirstOrDefault(unit => unit.HasResource == false);

        public void CreateNewUnit() => 
            _units.Add(_unitSpawner.SpawnNewUnit());

        public void SetDeliveryPositionForAllUnits(Vector3 flagPosition) => 
            _units.ForEach(unit => unit.SetDeliveryPosition(flagPosition));

        public void DisconnectUnit(Unit unit) => 
            _units.Remove(unit);

        public void AddUnit(Unit unit) => 
            _units.Add(unit);
    }
}