using ResourceComponents;
using UnityEngine;

namespace UnitComponents
{
    [RequireComponent(typeof(UnitMover))]
    [RequireComponent(typeof(ResourceCollector))]
    [RequireComponent(typeof(ResourceDetector))]
    public class Unit : MonoBehaviour
    {
        private UnitMover _mover;
        private ResourceDetector _resourceDetector;
        private ResourceCollector _resourceCollector;

        private Resource _currentResource;
    
        public bool HasResource => _currentResource != null;

        public void Awake()
        {
            _mover = gameObject.GetComponent<UnitMover>();
            _resourceDetector = gameObject.GetComponent<ResourceDetector>();
            _resourceCollector = gameObject.GetComponent<ResourceCollector>();
        }

        private void OnEnable()
        {
            _resourceDetector.Detected += OnResourceDetected;
            _resourceCollector.Collected += OnResourceCollected;
        }

        private void OnDisable()
        {
            _resourceDetector.Detected -= OnResourceDetected;
            _resourceCollector.Collected -= OnResourceCollected;
        }

        public void AcceptResource(Resource resource)
        {
            if (_currentResource != null)
                return;

            _currentResource = resource;
            _mover.MoveToResource(resource.transform.position);
        }

        public void Reset()
        {
            _currentResource = null;
            _mover.MoveToStartPosition();
        }

        private void OnResourceDetected(Resource resource)
        {
            if (_currentResource != resource)
                return;

            _resourceCollector.Collect(resource);
        }

        private void OnResourceCollected() => 
            _mover.MoveToBase();
    }
}