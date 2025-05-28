using System;
using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class PatrolState : WalkToPointState
    {
        readonly Action recalculatePatrolPoint;
        public PatrolState(TargetComponent _targetComponent, NavMeshAgent _navMeshAgent, Action _recalculatePatrolPoint) : base(_targetComponent, _navMeshAgent)
        {
            recalculatePatrolPoint = _recalculatePatrolPoint;
        }

        public override void StateEnter()
        {
            Debug.Log("Patrol State Enter");
            recalculatePatrolPoint.Invoke();
            base.StateEnter();
        }
    }
}