using System.Collections.Generic;

namespace FSM
{
    public abstract class State
    {
        private IStateChanger _stateChanger;
        private List<Transition> _transitions = new();

        protected State(IStateChanger stateChanger) =>
            _stateChanger = stateChanger;

        public void AddTransition(Transition transition) => 
            _transitions.Add(transition);

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public void Update()
        {
            foreach (Transition transition in _transitions)
            {
                if (transition.TryTransit(out State newState))
                {
                    _stateChanger.ChangeState(newState);

                    return;
                }
            }

            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}