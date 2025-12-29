using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ResourceDetector))]
public class ResourceCollector : MonoBehaviour
{
    [SerializeField] private Transform _attachPoint;
    [SerializeField] private float _collectingTime = 2f;
    
    private Resource _collectedResource;

    public bool IsCollected => _collectedResource != null;

    public void Collect(Resource resource)
    {
        StartCoroutine(Reparent(resource));
        
        _collectedResource = resource;
    }

    private IEnumerator Reparent(Resource resource)
    {
        yield return new WaitForSeconds(_collectingTime);
        
        resource.transform.SetParent(_attachPoint);
        resource.transform.position = _attachPoint.position;
    }
}