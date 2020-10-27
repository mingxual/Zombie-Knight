using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testNavAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    void Update()
    {
        agent.SetDestination(player.position);
    }
}
