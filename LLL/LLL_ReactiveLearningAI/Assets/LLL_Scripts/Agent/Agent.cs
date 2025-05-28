using System;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using LLL_Scripts.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace LLL_Scripts.Agent
{
    public class Agent : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] float movementSpeed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float patrolRadius;
        [SerializeField] float patrolPointDistanceThreshold;
        [Header("Detection")] [SerializeField] float detectionRadius;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] LayerMask obstructionMask;

        NavMeshAgent navAgent;
        State idleState;
        StateMachine.StateMachine stateMachine;
        TargetComponent idleTargetComponent;
        TargetComponent chaseTargetComponent;

        const int MaxPatrolPointAttempts = 300;

        void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.speed = movementSpeed;
            navAgent.angularSpeed = rotationSpeed;
            idleTargetComponent = new TargetComponent();
            chaseTargetComponent = new TargetComponent();
            var alertTimer = new Timer(5f);
            var idleTimer = new Timer(2f);
            
            idleState = new IdleState(idleTimer);
            stateMachine = new StateMachine.StateMachine(idleState);
            var patrolState = new PatrolState(idleTargetComponent, RecalculatePatrolPoint, navAgent);
            var chaseState = new WalkToPointState(chaseTargetComponent, navAgent);
            var returnToPointState = new WalkToPointState(idleTargetComponent, navAgent);
            var alertState = new IdleState(alertTimer);
            
            
            var idleToPatrol = new Transition(patrolState, idleTimer.CheckTimer);
            var idleToAlert = new Transition(alertState, FindTarget);
            var patrolToIdle = new Transition(idleState, HasReachedPosition);
            var patrolToChase = new Transition(chaseState, FindTarget);
            var chaseToReturn = new Transition(returnToPointState, () => !FindTarget());
            var returnToIdle = new Transition(idleState, HasReachedPosition);
            
            var alerToChase = new Transition(chaseState, alertTimer.CheckTimer);
            
            
            
            idleState.AddTransition(idleToAlert);
            idleState.AddTransition(idleToPatrol);
            
            patrolState.AddTransition(patrolToChase);
            patrolState.AddTransition(patrolToIdle);
            
            chaseState.AddTransition(chaseToReturn);

            returnToPointState.AddTransition(returnToIdle);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }

        bool HasReachedPosition()
        {
            return Vector3.Distance(transform.position, idleTargetComponent.TargetPosition) <= navAgent.stoppingDistance;
        }

        bool FindTarget()
        {
            var hit = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
            if (hit.Length <= 0) return false;
            var obstructionHit = Physics.Raycast(transform.position, (hit[0].transform.position - transform.position).normalized, 30f,  obstructionMask, QueryTriggerInteraction.Ignore);
            if (obstructionHit) return false;
            chaseTargetComponent.SetTarget(hit[0].transform);
            return true;
        }

        void RecalculatePatrolPoint()
        {
            Vector3 randomPoint;
            int attempts = 0;
            do
            {
                var unitSphere = Random.insideUnitSphere * patrolRadius;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                ++attempts;
            } while (!NavMesh.SamplePosition(randomPoint, out _, navAgent.radius * 2, navAgent.areaMask) || Vector3.Distance(transform.position, randomPoint) < patrolPointDistanceThreshold || attempts > MaxPatrolPointAttempts);
            idleTargetComponent.SetPoint(randomPoint);
        }
    }
}