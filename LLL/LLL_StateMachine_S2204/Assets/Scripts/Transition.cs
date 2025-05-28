using System;

namespace DefaultNamespace
{
    public class Transition
    {
        public readonly State NextState;
        public readonly Func<bool> Condition;

        public Transition(State _nextState, Func<bool> _condition)
        {
            NextState = _nextState;
            Condition = _condition;
        }
    }
}