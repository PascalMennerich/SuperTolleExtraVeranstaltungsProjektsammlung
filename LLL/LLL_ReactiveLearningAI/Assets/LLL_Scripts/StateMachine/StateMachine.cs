using Scriptables.Scripts;
using StateMachine;

namespace LLL_Scripts.StateMachine
{
    public class StateMachine
    {
        State currentState;
        

        public StateMachine(State _statState)
        {
            currentState = _statState;
        }

        public void CheckSwapState()
        {
            if (currentState.CheckTransition(out var nextState))
            {
                currentState.StateExit();
                currentState = nextState;
                currentState.StateEnter();
            }
            else
            {
                currentState.Tick();
            }
        }
    }
}