using Base;
using UnityEngine;
using UnityEngine.AI;

namespace UnitComponents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitMover : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        private Vector3 _resourceBasePosition;
        private Vector3 _startPosition;

        private void Awake() =>
            _navMeshAgent = GetComponent<NavMeshAgent>();

        private void Start()
        {
            _resourceBasePosition = FindObjectOfType<ResourceBase>().transform.position;
            _startPosition = transform.position;
        }

        public void MoveToStartPosition() =>
            _navMeshAgent.SetDestination(_startPosition);

        public void MoveToResource(Vector3 resourcePosition) =>
            _navMeshAgent.SetDestination(resourcePosition);

        public void MoveToBase() =>
            _navMeshAgent.SetDestination(_resourceBasePosition);
    }
}