using LL_Unity_Utils.Timers;
using UnityEngine;

namespace DefaultNamespace
{
    public class IdleState : State
    {

        Timer idleTimer;
        
        public IdleState(Timer _idleTimer)
        {
            idleTimer = _idleTimer;
        }
        
        public override void StateEnter()
        {
            Debug.Log("Enter Idle");
            idleTimer.StartTimer();
        }

        public override void StateExit()
        {
            Debug.Log("Exit Idle");
        }

        public override void Tick()
        {
            Debug.Log("IDLE :) ");
        }
    }
}