using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveAI : MonoBehaviour, IAIBehaviour
{
    private NavMeshAgent agent;
    public void Start()
    {
        Debug.Log("Passive Start");
        agent = GetComponent<NavMeshAgent>(); 
        agent.SetDestination(new Vector3(10, 0, 0));
    }

    public void Move()
    {
    }
}
