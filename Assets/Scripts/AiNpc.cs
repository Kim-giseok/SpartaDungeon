using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AiNpc : MonoBehaviour
{
    public NavMeshSurface surface;
    NavMeshAgent agent;

    public float speed;

    public  Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        surface.BuildNavMesh();
        agent.SetDestination(destination);
        if (Vector3.Distance(transform.position, destination) < 1)
            destination = NextDestination();
    }

    Vector3 NextDestination()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(Random.onUnitSphere * Random.Range(0, 40), out hit, 40, NavMesh.AllAreas);

        return hit.position;
    }
}
