using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class AggressiveAI : MonoBehaviour, IAIBehaviour
{
    private enum AggressiveState
    {
        GoingTo, Attacking, DoneAttacking, ReSearch
    }
    
    private AggressiveState _state = AggressiveState.GoingTo;
    private NavMeshAgent _agent;
    private GameObject _currentlyAttacking;
    private Unit.UnitOwner owner;
    public void Start()
    {
        owner = GetComponent<Unit>().Owner;
        _agent = GetComponent<NavMeshAgent>();
        FindAndGoToUnit();
    }

    private void FindAndGoToUnit()
    {
        var minDistance = math.INFINITY;
        foreach (var unit in AIManager.Instance.GetUnits())
        {
            if (unit == gameObject) continue;
            if (unit.GetComponent<Unit>().Owner == owner) continue;
            var dist = Vector3.Distance(unit.transform.position, gameObject.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                _currentlyAttacking = unit;
            }
        }

        if (_currentlyAttacking == null)
        {
            _state = AggressiveState.ReSearch;
            return;
        }

        _agent.SetDestination(_currentlyAttacking.transform.position);
        _agent.isStopped = false;
        _state = AggressiveState.GoingTo;
    }

    public void UpdateState()
    {
        if (_state == AggressiveState.GoingTo)
        {
            if (_agent.remainingDistance < 1)
            {
                // TODO - Attacking Script here!
                _agent.isStopped = true;
                return;
            }
            _agent.SetDestination(_currentlyAttacking.transform.position);
        }
        else if (_state == AggressiveState.Attacking)
        {
            // TODO - Check if enemy dies
        }
        else if (_state == AggressiveState.DoneAttacking)
        {
            // TODO - Go back to going to state
        } else if(_state == AggressiveState.ReSearch)
        {
            FindAndGoToUnit();
        }
    }



}
