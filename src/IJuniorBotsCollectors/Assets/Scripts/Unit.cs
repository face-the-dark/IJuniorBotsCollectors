using FSM;
using FSM.UnitFSM.Factory;
using UnityEngine;

[RequireComponent(typeof(UnitStateMachineFactory))]
[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(ResourceMaintainer))]
[RequireComponent(typeof(ResourceCollector))]
[RequireComponent(typeof(ResourceDetector))]
public class Unit : MonoBehaviour
{
    private StateMachine _stateMachine;

    private ResourceMaintainer _resourceMaintainer;
    
    public void Awake()
    {
        UnitMover mover = gameObject.GetComponent<UnitMover>();
        _resourceMaintainer = gameObject.GetComponent<ResourceMaintainer>();
        ResourceCollector resourceCollector = gameObject.GetComponent<ResourceCollector>();
        ResourceDetector resourceDetector = gameObject.GetComponent<ResourceDetector>();
        _stateMachine = GetComponent<UnitStateMachineFactory>()
            .Create(mover, _resourceMaintainer, resourceCollector, resourceDetector);
    }

    private void Update() => 
        _stateMachine?.Update();

    public void Reset() => 
        _resourceMaintainer.Clear();
}