using LL_Unity_Utils.Timers;

namespace LLL_Scripts.StateMachine
{
    public class IdleState : State
    {
        readonly Timer idleTimer;
        public IdleState(Timer _idleTimer)
        {
            idleTimer = _idleTimer;
        }
        
        public override void StateEnter()
        {
            idleTimer.StartTimer();
        }

        public override void StateExit()
        {
        }

        public override void Tick()
        {
        }
    }
}