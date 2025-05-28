using System.Collections.Generic;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using Scriptables.Scripts;
using StateMachine;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAgent : MonoBehaviour
    {
        [SerializeField] float movementSpeed;
        [SerializeField] float searchRadius;
        [SerializeField] float patrolRadius;
        [SerializeField] float patrolDistanceThreshold;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] List<StateHolder> stateHolders;
        [SerializeField] NavMeshSurface navMeshSurface;
        
        
        Timer idleTimer;
        State idleState;
        StateMachine.StateMachine stateMachine;

        NavMeshAgent navMeshAgent;
        TargetComponent targetComponent;

        Dictionary<EStateHolderType, StateHolder> stateHolderDict = new();
        
        //pickaxeID = 1700
        //waffe1 = 1800
        //waffe2 = 9821
        //Dictionary<int, int[]> miningOptions;
        //key: 1700, value [1700,1800,9821]

        void Awake()
        {
            
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = movementSpeed;
            idleTimer = new Timer(2f);
            idleState = new IdleState(idleTimer);
            // var stateMachine2 = new StateMachine.StateMachine(stateHolders[1]);
            targetComponent = new TargetComponent();

            var patrolState = new PatrolState(targetComponent, navMeshAgent, RecalculatePatrolPoint);
            var chaseState = new WalkToPointState(targetComponent, navMeshAgent);

            var idleToPatrol = new Transition(patrolState, () => idleTimer.CheckTimer());
            var idleToChase = new Transition(chaseState, FindTarget);
            var idleToSwapHolder = new Transition(null, () =>
            {
                //stateMachine = stateMachine2;
                var random = Random.Range(0, 2);
                if (random == 1) return false;
                stateMachine.ChangeStateHolder(stateHolders[1]);
                return true;
            });
            var chaseToIdle = new Transition(idleState, () => !FindTarget());
            var patrolToIdle = new Transition(idleState, () => Vector3.Distance(transform.position, targetComponent.TargetPosition) <= navMeshAgent.stoppingDistance);

            var anyToAngry = new Transition(null, () =>
            {
                //Funktion die alle Allies durchgeht und das Dictionary checkt:
                //stateHolderDict.ContainsKey(EStateHolderType.Angy)
                // if true:
                //stateMachine.ChangeStateHolder(stateHolderDict[EStateHolderType.Angy]);
                return true;
            });
            
            idleState.AddTransition(idleToChase);
            idleState.AddTransition(idleToSwapHolder);
            idleState.AddTransition(idleToPatrol);
            
            patrolState.AddTransition(patrolToIdle);
            
            chaseState.AddTransition(chaseToIdle);

            var boringState = new IdleState(new Timer(10000));
            
            // Fill stateholder
            
            stateHolders[0].AddState(idleState);
            stateHolders[0].AddState(patrolState);
            stateHolders[0].AddState(chaseState);
            
            stateHolders[1].AddState(boringState);
            
            stateMachine = new StateMachine.StateMachine(stateHolders[0]);
            
            stateHolderDict.Add(EStateHolderType.Normal, stateHolders[0]);
            stateHolderDict.Add(EStateHolderType.Boring, stateHolders[1]);
            stateHolderDict.Add(EStateHolderType.Angy, stateHolders[2]);
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
    
    public enum EStateHolderType{
    Normal,
    Boring,
    Angy
    }
}