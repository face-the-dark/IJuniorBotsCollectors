using Base;
using ResourceComponents;
using Spawner;
using UnityEngine;

namespace UnitComponents
{
    [RequireComponent(typeof(UnitMover))]
    [RequireComponent(typeof(ResourceCollector))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private ResourceBase _resourceBasePrefab;

        private const int ResultsSize = 3;
        private const float OverlapOffset = 1;

        private UnitMover _mover;
        private ResourceCollector _resourceCollector;

        private Resource _currentResource;

        private float _checkOverlapRadius = 1f;
        private Vector3 _deliveryPosition;
        private Vector3 _startPosition;

        private ResourceSpawner _resourceSpawner;
        private ResourceDatabase _resourceDatabase;

        public bool HasResource => _currentResource != null;

        public void Awake()
        {
            _mover = GetComponent<UnitMover>();
            _resourceCollector = GetComponent<ResourceCollector>();
        }

        private void Start() =>
            _startPosition = transform.position;

        private void OnEnable() =>
            _resourceCollector.Collected += OnResourceCollected;

        private void OnDisable() =>
            _resourceCollector.Collected -= OnResourceCollected;

        public void SetDeliveryPosition(Vector3 deliveryPosition) =>
            _deliveryPosition = deliveryPosition;

        public void UpdateStartPosition(Vector3 spawnPosition)
        {
            _startPosition = spawnPosition;

            if (_currentResource == null)
                _mover.MoveTo(_startPosition);
        }

        public void AcceptResource(Resource resource)
        {
            if (_currentResource != null)
                return;

            _currentResource = resource;
            _mover.MoveTo(resource.transform.position);
            _mover.Arrived += OnArrived;
        }

        public void Reset()
        {
            _currentResource = null;
            _resourceCollector.Reset();
            _mover.MoveTo(_startPosition);
            _mover.Arrived += OnArrived;
        }

        public void MoveToNewResourceBasePosition
        (
            Vector3 flagPosition,
            ResourceSpawner resourceSpawner,
            ResourceDatabase resourceDatabase
        )
        {
            _mover.MoveTo(flagPosition);
            _mover.Arrived += OnArrived;
            
            _resourceSpawner =  resourceSpawner;
            _resourceDatabase = resourceDatabase;
        }

        private void OnResourceCollected()
        {
            _mover.MoveTo(_deliveryPosition);
            _mover.Arrived += OnArrived;
        }

        private void OnArrived()
        {
            Collider[] results = new Collider[ResultsSize];

            Vector3 checkPosition = new Vector3(transform.position.x, transform.position.y - OverlapOffset,
                transform.position.z);
            Physics.OverlapSphereNonAlloc(checkPosition, _checkOverlapRadius, results);

            foreach (Collider result in results)
            {
                if (result && result.TryGetComponent(out Resource resource))
                {
                    _resourceCollector.Collect(resource);
                }
                else if (result && result.TryGetComponent(out ResourceBase resourceBase) &&
                         _resourceCollector.IsCollected)
                {
                    resourceBase.PickUpResource(this, _currentResource);
                }
                else if (result && result.TryGetComponent(out Flag flag))
                {
                    if (_resourceCollector.IsCollected)
                        flag.PickUpResource(this, _currentResource);
                    else
                        BuildNewResourceBase(flag);
                }
            }

            _mover.Arrived -= OnArrived;
        }

        private void BuildNewResourceBase(Flag flag)
        {
            ResourceBase resourceBase = Instantiate(_resourceBasePrefab, flag.transform.position, Quaternion.identity);
            resourceBase.Construct(_resourceSpawner, _resourceDatabase);
            Destroy(flag);

            resourceBase.JoinUnit(this);
        }
    }
}