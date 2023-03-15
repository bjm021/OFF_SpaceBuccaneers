using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveAI : MonoBehaviour, IAIBehaviour
{
    private NavMeshAgent agent;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        // TODO - Map End Marker or sth.
        if (GetComponent<Unit>().Owner == Unit.UnitOwner.PlayerOne) agent.SetDestination(new Vector3(-100, transform.position.y, transform.position.z));
        if (GetComponent<Unit>().Owner == Unit.UnitOwner.PlayerTwo) agent.SetDestination(new Vector3(100, transform.position.y, transform.position.z));

    }

    public void UpdateState()
    {
    }
}
