using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private MapConfig _mapConfig;
    [SerializeField] private ResourcePool _pool;
    
    [SerializeField] private float _delay = 2f;
    
    private Coroutine _spawnCoroutine;
    
    public event Action<Resource> ResourceSpawned;
    
    private void Start()
    {
        StopSpawnCoroutine();
        _spawnCoroutine = StartCoroutine(Spawn());
    }

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