using System.Collections;
using Base;
using Config;
using ResourceComponents;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Spawner
{
    public class ResourceSpawner : MonoBehaviour
    {
        private const int BaseCount = 1;

        [SerializeField] private ResourceBase _resourceBase;
        [SerializeField] private MapConfig _mapConfig;
        [SerializeField] private Resource resourcePrefab;
        [SerializeField] private float _delay = 2f;

        private ObjectPool<Resource> _pool;
        private Coroutine _spawnCoroutine;
        private BoxCollider _resourceBaseCollider;

        private void Awake()
        {
            _pool = new ObjectPool<Resource>(
                createFunc: Create,
                actionOnGet: InitResource,
                actionOnRelease: ResetResource,
                actionOnDestroy: Destroy
            );

            _resourceBaseCollider = _resourceBase.GetComponent<BoxCollider>();
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
                Vector3 newPosition = GeneratePosition();
                resource.transform.position = newPosition;

                yield return wait;
            }
        }

        private Vector3 GeneratePosition()
        {
            Collider[] baseColliders = new Collider[BaseCount];

            Vector3 position = GenerateRandomPosition();
            Physics.OverlapBoxNonAlloc(position, _resourceBaseCollider.size, baseColliders);

            while (baseColliders[0].GetComponent<ResourceBase>())
            {
                position = GenerateRandomPosition();
                Physics.OverlapBoxNonAlloc(position, _resourceBaseCollider.size, baseColliders);
            }

            return position;
        }

        private Vector3 GenerateRandomPosition()
        {
            float positionX = Random.Range(_mapConfig.MinEdgeSize, _mapConfig.MaxEdgeSize);
            float positionZ = Random.Range(_mapConfig.MinEdgeSize, _mapConfig.MaxEdgeSize);

            return new Vector3(positionX, 0, positionZ);
        }
    }
}