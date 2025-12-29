using UnityEngine;

public class ResourceMaintainer : MonoBehaviour
{
    [SerializeField] private ResourceBase _resourceBase;
    
    private Resource _currentResource;
    
    public Resource Resource => _currentResource;
    public bool HasResource => _currentResource != null;
    
    private void Update()
    {
        if (HasResource)
            return;
        
        _currentResource = _resourceBase.GetNonCollectedResource();
    }

    public void Clear() => 
        _currentResource = null;
}