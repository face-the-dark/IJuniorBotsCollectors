using FSM.UnitFSM.States;
using FSM.UnitFSM.Transitions;
using UnityEngine;

namespace FSM.UnitFSM.Factory
{
    public class UnitStateMachineFactory : MonoBehaviour
    {
        public StateMachine Create(UnitMover mover, ResourceMaintainer resourceMaintainer,
            ResourceCollector resourceCollector, ResourceDetector resourceDetector)
        {
            StateMachine stateMachine = new StateMachine();

            WaitState waitState = new WaitState(stateMachine);
            WalkState walkState = new WalkState(stateMachine, mover, resourceMaintainer);
            CollectState collectState = new CollectState(stateMachine, resourceMaintainer, resourceCollector);
            ReturnState returnState = new ReturnState(stateMachine, mover);

            ToWaitStateTransition toWaitStateTransition = new ToWaitStateTransition(waitState, resourceMaintainer);
            ToWalkStateTransition toWalkStateTransition = new ToWalkStateTransition(walkState, resourceMaintainer);
            ToCollectStateTransition toCollectStateTransition =
                new ToCollectStateTransition(collectState, resourceDetector);
            ToReturnStateTransition toReturnStateTransition = new ToReturnStateTransition(returnState, resourceCollector);

            waitState.AddTransition(toWalkStateTransition);
            walkState.AddTransition(toCollectStateTransition);
            collectState.AddTransition(toReturnStateTransition);
            returnState.AddTransition(toWaitStateTransition);

            stateMachine.ChangeState(waitState);

            return stateMachine;
        }
    }
}