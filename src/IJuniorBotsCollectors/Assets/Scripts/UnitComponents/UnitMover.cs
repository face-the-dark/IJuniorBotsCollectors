using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace UnitComponents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitMover : MonoBehaviour
    {
        [SerializeField] private float _distanceEpsilon = 0.01f;
        
        private NavMeshAgent _navMeshAgent;

        private Coroutine _arrivalCoroutine;

        public event Action Arrived;

        private void Awake() =>
            _navMeshAgent = GetComponent<NavMeshAgent>();

        public void MoveTo(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
            
            StopArrivalCoroutine();
            _arrivalCoroutine = StartCoroutine(ConfirmArrival());
        }

        private void StopArrivalCoroutine()
        {
            if (_arrivalCoroutine != null)
            {
                StopCoroutine(_arrivalCoroutine);
                _arrivalCoroutine = null;
            }
        }

        private IEnumerator ConfirmArrival()
        {
            while (_navMeshAgent.pathPending 
                   || _navMeshAgent.hasPath
                   || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance 
                   || _navMeshAgent.velocity.sqrMagnitude > _distanceEpsilon)
                yield return null;
            
            Arrived?.Invoke();
        }
    }
}