namespace FSM.UnitFSM.States
{
    public class ReturnState : State
    {
        private UnitMover _mover;
        
        public ReturnState(IStateChanger stateChanger, UnitMover mover) : base(stateChanger)
        {
            _mover = mover;
        }

        protected override void OnUpdate()
        {
            _mover.MoveToBase();
        }
    }
}