namespace FSM.UnitFSM.States
{
    public class WalkState : State
    {
        private UnitMover _mover;
        private ResourceMaintainer _resourceMaintainer;

        public WalkState
        (
            IStateChanger stateChanger,
            UnitMover mover,
            ResourceMaintainer resourceMaintainer
        ) : base(stateChanger)
        {
            _mover = mover;
            _resourceMaintainer = resourceMaintainer;
        }

        protected override void OnUpdate() => 
            _mover.MoveToResource(_resourceMaintainer.Resource);
    }
}