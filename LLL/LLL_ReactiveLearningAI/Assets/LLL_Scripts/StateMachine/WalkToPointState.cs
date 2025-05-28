using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace LLL_Scripts.StateMachine
{
    
    public class WalkToPointState : State
    {
        public const float AIRecalculationDistance = 0.5f;
        readonly TargetComponent targetComponent;
        NavMeshAgent navAgent;

        public WalkToPointState(TargetComponent _targetComponent, NavMeshAgent _agent)
        {
            targetComponent = _targetComponent;
            navAgent = _agent;
        }
        public override void StateEnter()
        {
            navAgent.SetDestination(targetComponent.TargetPosition);
        }

        public override void StateExit()
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }

        public override void Tick()
        {
            if (Vector3.Distance(targetComponent.TargetPosition, navAgent.destination) >= AIRecalculationDistance)
            {
                navAgent.SetDestination(targetComponent.TargetPosition);
            }
        }
    }
}