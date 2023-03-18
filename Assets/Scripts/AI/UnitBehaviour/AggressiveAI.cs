using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class AggressiveAI : MonoBehaviour, IAIBehaviour
{
    private enum AggressiveState
    {
        GoingTo, Attacking, DoneAttacking, ReSearch, WaitingForAttack
    }
    
    private AggressiveState _state = AggressiveState.GoingTo;
    private NavMeshAgent _agent;
    private GameObject _currentlyAttacking;
    private Unit _currentlyAttackingUnit;
    private Attack _attack;
    private Unit _unit;
    private GameManager.Player _owner;
    public void Start()
    {
        _owner = GetComponent<Unit>().Owner;
        _agent = GetComponent<NavMeshAgent>();
        _attack = GetComponent<Attack>();
        _unit = GetComponent<Unit>();
        FindAndGoToUnit();
    }

    private void FindAndGoToUnit()
    {
        var minDistance = math.INFINITY;
        Collider[] unitsInRange = Physics.OverlapSphere(gameObject.transform.position, _unit.UnitClass.AttackSeekRange, 1 << LayerMask.NameToLayer("Unit"));
        foreach (var unitCollider in unitsInRange)
        {
            GameObject unit = unitCollider.gameObject;
            if (unit == null) continue;
            Unit tmpUnit = unit.GetComponent<Unit>(); 
            if (unit == gameObject) continue;
            if (tmpUnit.Owner == _owner) continue;
            if (tmpUnit.Dead) continue; 
            var dist = Vector3.Distance(unit.transform.position, gameObject.transform.position);
            // MAYBE ERROR if (dist > _unit.UnitClass.AttackSeekRange) continue;
            if (dist < minDistance)
            {
                minDistance = dist;
                _currentlyAttacking = unit;
                _currentlyAttackingUnit = tmpUnit;
                _currentlyAttackingUnit.OnDeath.AddListener(EnterReSearchState);
            }
        }

        if (_currentlyAttacking == null)
        {
            EnterReSearchState();
            return;
        }

        if (_unit.Dead) return;
        _agent.SetDestination(_currentlyAttacking.transform.position);
        _agent.isStopped = false;
        _state = AggressiveState.GoingTo;
    }
    
    public void SearchNewTarget()
    {
        if (_unit.Dead) return;
        FindAndGoToUnit();
    }

    public void UpdateState()
    {
        if (_unit.Stunned || !_agent.enabled) return;
        if (_unit.Dead) return;
        var motherShipDist = Vector3.Distance(gameObject.transform.position, GameManager.Instance.GetEnemyMothership(_unit.Owner).transform.position);
        if (motherShipDist <= _unit.UnitClass.MothershipAttackDistance)
        {
            _agent.isStopped = true;
            _agent.speed = 0;
            AttackMotherhip(GameManager.Instance.GetEnemyMothership(_unit.Owner));
        }
        
        if (_state == AggressiveState.GoingTo)  
        {
            if (_currentlyAttackingUnit.Dead || _currentlyAttacking == null || _currentlyAttackingUnit == null) 
            {
                _state = AggressiveState.GoingTo;
                FindAndGoToUnit();
                return;
            }
            if (_agent.remainingDistance <= _attack.AttackRange)
            {
                DoAttack();
                return;
            }

            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_currentlyAttacking.transform.position);
            }
        }
        else if (_state == AggressiveState.Attacking)
        {
            if (_currentlyAttackingUnit.Dead || _currentlyAttacking == null || _currentlyAttackingUnit == null)
            {
                _state = AggressiveState.GoingTo;
                FindAndGoToUnit();
                return;
            }
            if (Vector3.Distance(_currentlyAttacking.transform.position, gameObject.transform.position) > _attack.AttackRange)
            {
                _state = AggressiveState.GoingTo;
                _agent.isStopped = false;
                _agent.SetDestination(_currentlyAttacking.transform.position);
                return;
            }
            DoAttack();
        }
        else if (_state == AggressiveState.DoneAttacking)
        {
            // TODO - Go back to going to state
        } 
        else if(_state == AggressiveState.ReSearch)
        {
            FindAndGoToUnit();
        }
        else if (_state == AggressiveState.WaitingForAttack)
        {
            DoAttack();
        }
    }

    private void DoAttack()
    {
        if (_currentlyAttackingUnit.Dead) 
        {
            _state = AggressiveState.GoingTo;
            FindAndGoToUnit();
            return;
        }
        if (!_attack.IsReady())
        {
            _state = AggressiveState.WaitingForAttack;
            return;
        }
        if (!_unit.Dead) _agent.isStopped = true; 
        var dead = _attack.DoAttack(_currentlyAttacking, _unit);

        if (dead)
        {
            _state = AggressiveState.ReSearch;
            return;
        }
        else
        {
            _state = AggressiveState.Attacking;
        }
    }


    private void EnterReSearchState()
    {
        if (_unit.Dead || !_agent.isOnNavMesh) return;
        _state = AggressiveState.ReSearch;
        if (_unit.Owner == GameManager.Player.PlayerTwo) _agent.SetDestination(new Vector3(-100, transform.position.y, transform.position.z));
        if (_unit.Owner == GameManager.Player.PlayerOne) _agent.SetDestination(new Vector3(100, transform.position.y, transform.position.z));
        _agent.isStopped = false;
    }


    public bool AttackMotherhip(GameObject mothership)
    {
        return _attack.AttackMothership(mothership, _unit);
    }


}
