using UnityEngine;
using UnityEngine.Pool;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] private Resource resourcePrefab;
    
    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: Create,
            actionOnGet: SetActiveTrue,
            actionOnRelease: SetActiveFalse,
            actionOnDestroy: Destroy
        );
    }

    public Resource Get() => 
        _pool.Get();

    public void Release(Resource resource)
    {
        _pool.Release(resource);
        resource.Reset();
    }

    private Resource Create() => 
        Instantiate(resourcePrefab, transform.position, Quaternion.identity);

    private void SetActiveTrue(Resource resource) => 
        resource.gameObject.SetActive(true);

    private void SetActiveFalse(Resource resource) => 
        resource.gameObject.SetActive(false);
}