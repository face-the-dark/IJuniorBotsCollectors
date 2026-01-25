using System;
using Spawner;
using UnitComponents;
using UnityEngine;

namespace Base
{
    public class ResourceBaseBuilder : MonoBehaviour
    {
        [SerializeField] private ResourceBase _resourceBasePrefab;
        [SerializeField] private ResourceSpawner _resourceSpawner;
        [SerializeField] private ResourceDatabase _resourceDatabase;

        public event Action Built;

        private void Start()
        {
            ResourceBase resourceBase = Build(transform.position);
            resourceBase.SpawnStartUnits();
        }

        public ResourceBase Build(Vector3 buildPosition, Unit unit = null)
        {
            ResourceBase resourceBase = Instantiate(_resourceBasePrefab, buildPosition, Quaternion.identity);
            resourceBase.Construct(_resourceSpawner, _resourceDatabase, this);
            
            if (unit)
                resourceBase.JoinUnit(unit);
            
            Built?.Invoke();
            
            return resourceBase;
        }
    }
}