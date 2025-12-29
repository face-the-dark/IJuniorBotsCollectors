namespace FSM.UnitFSM.Transitions
{
    public class ToCollectStateTransition : Transition
    {
        private ResourceDetector _resourceDetector;
        
        public ToCollectStateTransition(State nextState, ResourceDetector resourceDetector) : base(nextState)
        {
            _resourceDetector = resourceDetector;
        }

        protected override bool CanTransit()
        {
            return _resourceDetector.IsTriggered;
        }
    }
}