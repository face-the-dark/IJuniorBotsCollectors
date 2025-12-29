namespace FSM.UnitFSM.States
{
    public class CollectState : State
    {
        private ResourceCollector _resourceCollector;
        private ResourceMaintainer _resourceMaintainer;

        public CollectState
        (
            IStateChanger stateChanger,
            ResourceMaintainer resourceMaintainer,
            ResourceCollector resourceCollector
        ) : base(stateChanger)
        {
            _resourceCollector = resourceCollector;
            _resourceMaintainer = resourceMaintainer;
        }

        public override void Enter() => 
            _resourceCollector.Collect(_resourceMaintainer.Resource);
    }
}