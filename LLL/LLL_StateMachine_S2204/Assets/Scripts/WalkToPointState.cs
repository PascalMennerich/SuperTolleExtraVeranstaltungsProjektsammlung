using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class WalkToPointState : State
    {
        public const float AIRecalculationDistance = 0.5f;
        TargetComponent targetComponent;
        NavMeshAgent navMeshAgent;

        public WalkToPointState(TargetComponent _targetComponent, NavMeshAgent _navMeshAgent)
        {
            targetComponent = _targetComponent;
            navMeshAgent = _navMeshAgent;
        }

        public override void StateEnter()
        {
            Debug.Log("WalkToPointState Enter");
        }

        public override void StateExit()
        {
            Debug.Log("WalkToPointState Exit");
        }

        public override void Tick()
        {
            if (Vector3.Distance(targetComponent.TargetPosition, navMeshAgent.destination) >= AIRecalculationDistance)
            {
                navMeshAgent.SetDestination(targetComponent.TargetPosition);
            }
        }
    }
}