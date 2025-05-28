using System;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAgent : MonoBehaviour
    {
        // Movementspeed hier hat den Vorteil für dynmaischerer Einstellung im Vergleich zum einstellen im Inspektor beim NavMeshAgent
        [SerializeField] float movementSpeed;
        [SerializeField] float searchRadius;
        [SerializeField] float patrolRadius;
        [SerializeField] float patrolDistanceThreshold;
        [SerializeField] LayerMask detectionMask;

        Timer idleTimer;
        State idleState;
        StateMachine stateMachine;

        NavMeshAgent navMeshAgent;
        TargetComponent targetComponent;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = movementSpeed;
            idleTimer = new Timer(2f);
            idleState = new IdleState(idleTimer);
            stateMachine = new StateMachine(idleState);
            targetComponent = new TargetComponent();

            var patrolState = new PatrolState(targetComponent, navMeshAgent, RecalculatePatrolPoint);
            var chaseState = new WalkToPointState(targetComponent, navMeshAgent);

            var idleToPatrol = new Transition(patrolState, () => idleTimer.CheckTimer());
            var idleToChase = new Transition(chaseState, FindTarget);
            var chaseToIdle = new Transition(idleState, () => !FindTarget());
            var patrolToIdle = new Transition(idleState, () => Vector3.Distance(transform.position, targetComponent.TargetPosition) <= navMeshAgent.stoppingDistance);
            
            idleState.AddTransition(idleToChase);
            idleState.AddTransition(idleToPatrol);
            
            patrolState.AddTransition(patrolToIdle);
            
            chaseState.AddTransition(chaseToIdle);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }


        bool FindTarget()
        {
            var overlap = Physics.OverlapSphere(transform.position, searchRadius, detectionMask);
            if (overlap.Length > 0)
            {
                targetComponent.SetTarget(overlap[0].transform);
                return true;
            }

            return false;
        }

        void RecalculatePatrolPoint()
        {
            Vector3 randomPoint;
            do
            {
                var unitSphere = Random.insideUnitSphere * patrolRadius;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
            } while (!NavMesh.SamplePosition(randomPoint, out _, navMeshAgent.radius * 2, navMeshAgent.areaMask) || Vector3.Distance(transform.position, randomPoint) < patrolDistanceThreshold);

            targetComponent.SetPoint(randomPoint);
        }
    }
}