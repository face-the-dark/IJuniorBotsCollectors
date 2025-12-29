using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMover : MonoBehaviour
{
    [SerializeField] private ResourceBase _resourceBase;
    
    private NavMeshAgent _navMeshAgent;

    private void Awake() => 
        _navMeshAgent = GetComponent<NavMeshAgent>();

    public void MoveToResource(Resource resource) => 
        _navMeshAgent.SetDestination(resource.transform.position);

    public void MoveToBase() => 
        _navMeshAgent.SetDestination(_resourceBase.transform.position);
}