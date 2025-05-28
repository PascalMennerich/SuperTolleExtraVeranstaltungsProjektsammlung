
using Scriptables.Scripts;

namespace StateMachine
{
    public class StateMachine
    {
        StateHolder stateHolder;
        StateHolder defaultHolder;

        public StateMachine(StateHolder _stateHolder)
        {
            stateHolder = _stateHolder;
            stateHolder.ChangeState(stateHolder.GetFirstState());
        }

        public void ChangeStateHolder(StateHolder _newStateHolder)
        {
            stateHolder = _newStateHolder;
        }

        public void CheckSwapState()
        {
            if (stateHolder.CurrentState.CheckTransition(out var nextState))
            {
                stateHolder.CurrentState.StateExit();
                stateHolder.ChangeState(nextState);
                stateHolder.CurrentState.StateEnter();
            }
            else
            {
                stateHolder.CurrentState.Tick();
            }
        }
    }
}

