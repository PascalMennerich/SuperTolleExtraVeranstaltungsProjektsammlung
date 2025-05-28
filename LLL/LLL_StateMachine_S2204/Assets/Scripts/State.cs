using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public abstract class State
    {
        List<Transition> transitions;

        public State()
        {
            transitions = new List<Transition>();
        }

        public abstract void StateEnter();
        public abstract void StateExit();
        public abstract void Tick();

        public void AddTransition(Transition _transition)
        {
            transitions.Add(_transition);
        }

        public void RemoveTransition(Transition _transition)
        {
            transitions.Remove(_transition);
        }

        public bool CheckTransition(out State _nextState)
        {
            foreach (var transition in transitions.Where(_transition => _transition.Condition()))
            {
                _nextState = transition.NextState;
                return true;
            }
            _nextState = null;
            return false;
        }
    }
}