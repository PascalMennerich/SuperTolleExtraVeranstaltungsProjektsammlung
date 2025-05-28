using System;
using LL_Unity_Utils.Misc;
using UnityEngine.AI;

namespace LLL_Scripts.StateMachine
{
    public class PatrolState : WalkToPointState
    {
        TargetComponent idleTargetComponent;
        readonly Action recalculateFunction;
        public PatrolState(TargetComponent _targetComponent, Action _recalculateFunction, NavMeshAgent _agent) : base(_targetComponent, _agent)
        {
            recalculateFunction = _recalculateFunction;
        }

        public override void StateEnter()
        {
            recalculateFunction.Invoke();
            base.StateEnter();
        }
    }
}