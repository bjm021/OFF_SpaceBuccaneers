using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class SpecialPrioritisingAggressiveAI : MonoBehaviour, IAIBehaviour
{
    private enum SPAAIState
    {
        GoingTo, Attacking, DoneAttacking, ReSearch, WaitingForAttack
    }
    
    private SPAAIState _state = SPAAIState.GoingTo;
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
        Debug.Log("Searching for special unit");
        var minDistance = math.INFINITY;
        
        Collider[] unitsInRange = Physics.OverlapSphere(gameObject.transform.position, _unit.UnitClass.AttackSeekRange, 1 << LayerMask.NameToLayer("Unit"));
        
        foreach (var unitCollider in unitsInRange)
        {
            GameObject unit = unitCollider.gameObject;
            if (unit == null) continue;
            Unit tmpUnit = unit.GetComponent<Unit>();
            if (!tmpUnit.UnitClass.Special) continue;
            if (unit == gameObject) continue;
            if (tmpUnit.Owner == _owner) continue;
            if (tmpUnit.Dead) continue; 
            var dist = Vector3.Distance(unit.transform.position, gameObject.transform.position);
            if (dist > _unit.UnitClass.AttackSeekRange) continue;
            if (dist < minDistance)
            {
                minDistance = dist;
                _currentlyAttacking = unit;
                _currentlyAttackingUnit = tmpUnit;
            }
        }


        if (_currentlyAttacking == null)
        {
            Debug.Log("No Special unit found, searching for normal unit");
            foreach (var unitCollider in unitsInRange)
            {
                GameObject unit = unitCollider.gameObject;
                if (unit == null) continue;
                Unit tmpUnit = unit.GetComponent<Unit>(); 
                if (unit == gameObject) continue;
                if (tmpUnit.Owner == _owner) continue;
                if (tmpUnit.Dead) continue; 
                var dist = Vector3.Distance(unit.transform.position, gameObject.transform.position);
                if (dist > _unit.UnitClass.AttackSeekRange) continue;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    _currentlyAttacking = unit;
                    _currentlyAttackingUnit = tmpUnit;
                }
            }
        }

        if (_currentlyAttacking == null)
        {
            EnterReSearchState();
            return;
        }

        _agent.SetDestination(_currentlyAttacking.transform.position);
        _agent.isStopped = false;
        _state = SPAAIState.GoingTo;
    }

    public void UpdateState()
    {
        var motherShipDist = Vector3.Distance(gameObject.transform.position, GameManager.Instance.GetEnemyMothership(_unit.Owner).transform.position);
        if (motherShipDist <= _unit.UnitClass.MothershipAttackDistance)
        {
            Debug.Log("Attacking mothership");
            _agent.isStopped = true;
            _agent.speed = 0;
            AttackMotherhip(GameManager.Instance.GetEnemyMothership(_unit.Owner));
        }
        
        if (_state == SPAAIState.GoingTo)  
        {
            if (_currentlyAttackingUnit.Dead || _currentlyAttacking == null || _currentlyAttackingUnit == null) 
            {
                _state = SPAAIState.GoingTo;
                FindAndGoToUnit();
                return;
            }
            if (_agent.remainingDistance <= _attack.AttackRange)
            {
                DoAttack();
                return;
            }
            _agent.isStopped = false;
            _agent.SetDestination(_currentlyAttacking.transform.position);
        }
        else if (_state == SPAAIState.Attacking)
        {
            if (_currentlyAttackingUnit.Dead || _currentlyAttacking == null || _currentlyAttackingUnit == null)
            {
                _state = SPAAIState.GoingTo;
                FindAndGoToUnit();
                return;
            }
            if (Vector3.Distance(_currentlyAttacking.transform.position, gameObject.transform.position) > _attack.AttackRange)
            {
                _state = SPAAIState.GoingTo;
                _agent.isStopped = false;
                _agent.SetDestination(_currentlyAttacking.transform.position);
                return;
            }
            DoAttack();
        }
        else if (_state == SPAAIState.DoneAttacking)
        {
            // TODO - Go back to going to state
        } 
        else if(_state == SPAAIState.ReSearch)
        {
            FindAndGoToUnit();
        }
        else if (_state == SPAAIState.WaitingForAttack)
        {
            DoAttack();
        }
    }

    private void DoAttack()
    {
        if (_currentlyAttackingUnit.Dead) 
        {
            _state = SPAAIState.GoingTo;
            FindAndGoToUnit();
            return;
        }
        if (!_attack.IsReady())
        {
            _state = SPAAIState.WaitingForAttack;
            return;
        }
        _agent.isStopped = true; 
        var dead = _attack.DoAttack(_currentlyAttacking);

        if (dead)
        {
            _state = SPAAIState.ReSearch;
            return;
        }
        else
        {
            _state = SPAAIState.Attacking;
        }
    }


    private void EnterReSearchState()
    {
        _state = SPAAIState.ReSearch;
        if (_unit.Owner == GameManager.Player.PlayerTwo) _agent.SetDestination(new Vector3(-100, transform.position.y, transform.position.z));
        if (_unit.Owner == GameManager.Player.PlayerOne) _agent.SetDestination(new Vector3(100, transform.position.y, transform.position.z));
        _agent.isStopped = false;
    }


    public bool AttackMotherhip(GameObject mothership)
    {
        return _attack.AttackMothership(mothership);
    }


}
