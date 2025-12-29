namespace FSM.UnitFSM.Transitions
{
    public class ToReturnStateTransition : Transition
    {
        private ResourceCollector _resourceCollector;
        
        public ToReturnStateTransition(State nextState, ResourceCollector resourceCollector) : base(nextState)
        {
            _resourceCollector = resourceCollector;
        }

        protected override bool CanTransit()
        {
            return _resourceCollector.IsCollected;
        }
    }
}