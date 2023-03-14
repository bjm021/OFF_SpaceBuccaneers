using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public enum UnitOwner
    {
        Player,
        Enemy
    }
    

    public UnitClass UnitClass { get; private set; }
    public UnitOwner Owner { get; private set; }
    public IAIBehaviour BehaviourScript { get; private set; }
    public bool Dead { get; private set; }

    private int _currentHealth;
    private NavMeshAgent _navMeshAgent;
    private Attack _attack;
    
    public void Initialize(UnitClass unitClass, UnitOwner owner)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attack = GetComponent<Attack>();
        
        UnitClass = unitClass;
        Owner = owner;
        Dead = false;
        
        _currentHealth = UnitClass.Health;
        
        _navMeshAgent.speed = UnitClass.MoveSpeed;
        _navMeshAgent.stoppingDistance = UnitClass.AttackRange - 1;
        
        _attack.Initialize(UnitClass.Attack, UnitClass.AttackCooldown, UnitClass.AttackRange);

        BehaviourScript = UnitClass.behaviour switch
        {
            UnitClass.AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
            UnitClass.AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
            UnitClass.AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
            UnitClass.AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
            UnitClass.AIBehaviourType.SpecialMining => gameObject.AddComponent<SpecialMiningAI>(),
            _ => BehaviourScript 
        };
        AIManager.Instance.AddUnit(gameObject);
        BehaviourScript.Start();
    }

    public int TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        return _currentHealth;
    }

    private void Die()
    {
        AIManager.Instance.RemoveUnit(gameObject);
        Destroy(gameObject);
    }
}