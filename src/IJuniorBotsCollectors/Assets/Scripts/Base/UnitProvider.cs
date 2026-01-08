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
    }
}