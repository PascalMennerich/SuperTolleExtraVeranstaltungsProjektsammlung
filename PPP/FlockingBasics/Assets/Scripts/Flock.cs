#define YEET

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    /* FLOCKING
     Besteht aus mindestens 3 "Kräften" - COHESION - SEPARATION - ALIGNMENT
     COHESION: Wie sehr möchte ich mit meinen "Nachbarn" zusammen bleiben?
     SEPARATION: Wie sehr möchte ich von meinen "Nachbarn" weg?
     ALIGNMENT: Wie sehr möchte ich mich an der Bewegungsrichtung meiner "Nachbarn" orientieren?
    */

    [SerializeField] private int updatesPerSecond;
    [SerializeField] private int agentCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float checkRadius;
    [SerializeField] private float agentSpeed;
    [SerializeField] private FlockingAgent agentPrefab;
    [SerializeField] private float cohesionStrength;
    [SerializeField] private float separationStrength;
    [SerializeField] private float alignmentStrength;
    [SerializeField] private float rubberbandStrength;
    [SerializeField] private LayerMask agentLayerMask;

    private List<FlockingAgent> agents;

    private IEnumerator Start()
    {
        agents = new List<FlockingAgent>();
        YieldInstruction delay = new WaitForSeconds(1f / updatesPerSecond);

        for (int i = 0; i < agentCount; i++)
        {
            agents.Add(Instantiate(agentPrefab, transform.position + Random.insideUnitSphere * spawnRadius,
                    Random.rotation)
                .Initialize(checkRadius, cohesionStrength, separationStrength, alignmentStrength, agentLayerMask, agentSpeed, this, rubberbandStrength));
        }

        while (true)
        {
            for (int i = 0; i < agentCount; i++)
            {
                agents[i].ApplyForce(1f / updatesPerSecond);
            }

            yield return delay;
        }
    }
}