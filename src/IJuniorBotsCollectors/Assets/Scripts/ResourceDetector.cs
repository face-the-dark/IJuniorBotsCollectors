using UnityEngine;

[RequireComponent(typeof(ResourceMaintainer))]
public class ResourceDetector : MonoBehaviour
{
    private ResourceMaintainer _resourceMaintainer;

    public bool IsTriggered;

    private void Awake()
    {
        _resourceMaintainer = GetComponent<ResourceMaintainer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && _resourceMaintainer.Resource == resource)
        {
            IsTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other) =>
        IsTriggered = false;
}