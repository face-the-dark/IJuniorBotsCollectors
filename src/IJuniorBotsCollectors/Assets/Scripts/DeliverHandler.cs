using System;
using UnityEngine;

public class DeliverHandler : MonoBehaviour
{
    public event Action<Unit, Resource> UnitDelivered; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            Resource resource = unit.GetComponentInChildren<Resource>();
            
            if (resource != null)
                UnitDelivered?.Invoke(unit, resource);
        }
    }
}