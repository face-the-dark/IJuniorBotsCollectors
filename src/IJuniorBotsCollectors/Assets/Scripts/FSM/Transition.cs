namespace FSM
{
    public abstract class Transition
    {
        private State _nextState;

        protected Transition(State nextState) => 
            _nextState = nextState;

        public bool TryTransit(out State nextState)
        {
            nextState = _nextState;

            return CanTransit();
        }

        protected abstract bool CanTransit();
    }
}