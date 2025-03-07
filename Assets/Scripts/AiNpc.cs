using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AiNpc : MonoBehaviour
{
    NavMeshSurface surface;
    NavMeshAgent agent;

    public float speed;

    public  Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        surface.BuildNavMesh();
        agent.SetDestination(destination);
    }
}
