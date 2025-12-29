namespace FSM.UnitFSM.Transitions
{
    public class ToWalkStateTransition : Transition
    {
        private ResourceMaintainer _resourceMaintainer;
        
        public ToWalkStateTransition(State nextState, ResourceMaintainer resourceMaintainer) : base(nextState) => 
            _resourceMaintainer = resourceMaintainer;

        protected override bool CanTransit() => 
            _resourceMaintainer.HasResource;
    }
}