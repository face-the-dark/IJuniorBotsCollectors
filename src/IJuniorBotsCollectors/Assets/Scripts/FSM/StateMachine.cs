namespace FSM
{
    public class StateMachine : IStateChanger
    {
        private State _currentState;
        
        public void ChangeState(State state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}