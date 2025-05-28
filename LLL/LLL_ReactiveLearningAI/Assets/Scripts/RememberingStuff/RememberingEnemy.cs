using Unity.AI.Navigation;
using UnityEngine;

namespace RememberingStuff
{
    public class RememberingEnemy : MonoBehaviour
    {
        EnemyBrain myBrain;
        [SerializeField] NavMeshSurface  mySurface;
        void Awake()
        {
            mySurface.BuildNavMesh();
            myBrain = new EnemyBrain();
            myBrain.AddMemoryCategory(ERememberanceType.AmountOfPatrolPointsReached, 0);
            myBrain.AddMemoryCategory(ERememberanceType.TimeChased, 0);
            myBrain.AddMemoryCategory(ERememberanceType.DamageTaken, 0);
            
            myBrain.GetMemoryCategory(ERememberanceType.AmountOfPatrolPointsReached, out var value);
            Debug.Log(value);
            myBrain.UpdateMemoryCategory(ERememberanceType.TimeChased, 5);
            myBrain.GetMemoryCategory(ERememberanceType.TimeChased, out var value2);
            Debug.Log(value2);
            myBrain.GetMemoryCategory(ERememberanceType.DamageTaken, out var value3);
            Debug.Log(value3);
        }
    }

    public enum ERememberanceType
    {
        AmountOfPatrolPointsReached,
        TimeChased,
        DamageTaken
    }
}