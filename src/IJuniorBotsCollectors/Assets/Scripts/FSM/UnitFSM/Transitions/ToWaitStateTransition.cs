namespace FSM.UnitFSM.Transitions
{
    public class ToWaitStateTransition : Transition
    {
        private ResourceMaintainer _resourceMaintainer;
        
        public ToWaitStateTransition(State nextState, ResourceMaintainer resourceMaintainer) : base(nextState) => 
            _resourceMaintainer = resourceMaintainer;

        protected override bool CanTransit() => 
            _resourceMaintainer.HasResource == false;
    }
}