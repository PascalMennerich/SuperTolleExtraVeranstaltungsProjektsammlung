using System.Collections.Generic;
using UnityEngine;

public class FlockingAgent : MonoBehaviour
{
    private float checkRadius;
    private float cohesionStrength;
    private float separationStrength;
    private float alignmentStrength;
    private float rubberbandStrength;
    private float agentSpeed;

    private LayerMask agentLayerMask;
    private Vector3 direction;
    private List<FlockingAgent> neighbors;
    private Flock flock;

    public FlockingAgent Initialize(float checkRadius, float cohesionStrength, float separationStrength,
        float alignmentStrength, LayerMask layerMask, float agentSpeed, Flock flock, float rubberbandStrength)
    {
        this.checkRadius = checkRadius;
        this.cohesionStrength = cohesionStrength;
        this.separationStrength = separationStrength;
        this.alignmentStrength = alignmentStrength;
        this.agentSpeed = agentSpeed;
        this.rubberbandStrength = rubberbandStrength;
        this.flock = flock;
        agentLayerMask = layerMask;

        return this;
    }

    public void ApplyForce(float time)
    {
        GetNeighbours();

        direction = Vector3.zero;
        direction += GetCohesionVector() * cohesionStrength;
        direction += GetSeparationVector() * separationStrength;
        direction += GetAlignmentVector() * alignmentStrength;

        float flockDistance = (flock.transform.position - transform.position).magnitude;
        
        if (flockDistance > 10)
        {
            direction += (flock.transform.position - transform.position) * rubberbandStrength;
            direction /= 4f;
        }
        else
        {
            direction /= 3f;
        }
        
        transform.position += direction * time * agentSpeed;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime);
    }

    private void GetNeighbours()
    {
        neighbors = new List<FlockingAgent>();
        Collider[] neighborColliders = Physics.OverlapSphere(transform.position, checkRadius, agentLayerMask);

        for (int i = 0; i < neighborColliders.Length; i++)
        {
            neighbors.Add(neighborColliders[i].gameObject.GetComponent<FlockingAgent>());
        }
    }

    private Vector3 GetCohesionVector()
    {
        Vector3 cohesionVector = Vector3.zero;

        for (int i = 0; i < neighbors.Count; i++)
        {
            cohesionVector += neighbors[i].transform.position - transform.position;
        }

        cohesionVector /= neighbors.Count;

        return cohesionVector;
    }

    private Vector3 GetSeparationVector()
    {
        Vector3 separationVector = Vector3.zero;

        for (int i = 0; i < neighbors.Count; i++)
        {
            separationVector += transform.position - neighbors[i].transform.position;
        }

        separationVector /= neighbors.Count;

        return separationVector;
    }

    private Vector3 GetAlignmentVector()
    {
        Vector3 alignmentVector = transform.forward;

        for (int i = 0; i < neighbors.Count; i++)
        {
            alignmentVector += neighbors[i].transform.forward;
        }

        alignmentVector /= neighbors.Count;

        return alignmentVector;
    }
}