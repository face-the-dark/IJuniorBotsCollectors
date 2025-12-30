using System;
using ResourceComponents;
using UnityEngine;

namespace UnitComponents
{
    public class ResourceDetector : MonoBehaviour
    {
        public event Action<Resource> Detected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Resource resource))
                Detected?.Invoke(resource);
        }
    }
}