using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveComponent : EntityComponent
{
    private NavMeshAgent Agent;
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 PointToMove)
    {
        Agent.SetDestination(PointToMove);
    }
}
