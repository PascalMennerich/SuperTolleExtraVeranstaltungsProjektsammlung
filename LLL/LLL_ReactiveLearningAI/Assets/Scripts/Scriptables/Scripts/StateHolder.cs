using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Scriptables.Scripts
{
    [CreateAssetMenu(menuName = "Scriptables/StateMachine/StateHolder",  fileName = "NewStateHolder")]
    public class StateHolder : ScriptableObject
    {
        readonly List<State> states = new List<State>();

        State currentState;
        public State CurrentState
        {
            get => currentState ?? states[0];
            private set => currentState = value;
        }


        public void AddState(State _state)
        {
            states.Add(_state);
        }

        public void RemoveState(State _state)
        {
            states.Remove(_state);
        }

        public void ChangeState(State _state)
        {
            CurrentState = _state;
        }

        public State GetFirstState()
        {
            return states[0];
        }
        
    }
    
    
}
