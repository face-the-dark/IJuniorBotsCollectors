using System;
using System.Collections;
using Config;
using ResourceComponents;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Spawner
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private MapConfig _mapConfig;
        [SerializeField] private Resource resourcePrefab;
        [SerializeField] private float _delay = 2f;

        private ObjectPool<Resource> _pool;
        private Coroutine _spawnCoroutine;
    
        public event Action<Resource> ResourceSpawned;
    
        private void Awake()
        {
            _pool = new ObjectPool<Resource>(
                createFunc: Create,
                actionOnGet: InitResource,
                actionOnRelease: ResetResource,
                actionOnDestroy: Destroy
            );
        }
    
        private void Start()
        {
            StopSpawnCoroutine();
            _spawnCoroutine = StartCoroutine(Spawn());
        }

        public void Release(Resource resource) => 
            _pool.Release(resource);

        private Resource Create() => 
            Instantiate(resourcePrefab, transform.position, Quaternion.identity);

        private void InitResource(Resource resource) =>
            resource.Init();

        private void ResetResource(Resource resource) => 
            resource.Reset();

        private void StopSpawnCoroutine()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }

        private IEnumerator Spawn()
        {
            WaitForSeconds wait = new WaitForSeconds(_delay);

            while (enabled)
            {
                Resource resource = _pool.Get();
                Vector3 newPosition = GenerateRandomPosition();
                resource.transform.position = newPosition;
            
                ResourceSpawned?.Invoke(resource);

                yield return wait;
            }
        }

        private Vector3 GenerateRandomPosition()
        {
            float positionX = Random.Range(_mapConfig.MinEdgeSize, _mapConfig.MaxEdgeSize);
            float positionZ = Random.Range(_mapConfig.MinEdgeSize, _mapConfig.MaxEdgeSize);
        
            return new Vector3(positionX, 0, positionZ);
        }
    }
}