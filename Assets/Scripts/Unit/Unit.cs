using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public UnitClass UnitClass { get; set; }
    public GameManager.Player Owner { get; set; }
    public IAIBehaviour BehaviourScript { get; set; }
    public bool Dead { get; private set; }
    public UnitSpawner SpawnedBy { get; private set; } = null;
    
    public UnityEvent OnDeath = new UnityEvent(); 

    private int _currentHealth;
    private NavMeshAgent _navMeshAgent;
    private Attack _attack;
    private SphereCollider _viewTrigger;
    
    public void Initialize(UnitClass unitClass, GameManager.Player owner, UnitSpawner spawnedBy)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attack = GetComponent<Attack>();
        _viewTrigger = GetComponent<SphereCollider>();
        
        UnitClass = unitClass;
        Owner = owner;
        Dead = false;
        SpawnedBy = spawnedBy;

        _currentHealth = UnitClass.Health;
        
        _navMeshAgent.speed = UnitClass.MoveSpeed;
        _navMeshAgent.stoppingDistance = UnitClass.AttackRange - 1;
        
        _viewTrigger.radius = UnitClass.AttackSeekRange;
        
        _attack.Initialize(UnitClass.Attack, UnitClass.AttackCooldown, UnitClass.AttackRange);

        BehaviourScript = UnitClass.behaviour switch
        {
            UnitClass.AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
            UnitClass.AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
            UnitClass.AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
            UnitClass.AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
            UnitClass.AIBehaviourType.SpecialMining => gameObject.AddComponent<SpecialMiningAI>(),
            UnitClass.AIBehaviourType.SpecialPrioritisingAggressive => gameObject.AddComponent<SpecialPrioritisingAggressiveAI>(),
            _ => BehaviourScript 
        };

        BehaviourScript.Start();
    }

    private IEnumerator UpdateAI()
    {
        while (true)
        {
            BehaviourScript.UpdateState();
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // TODO - Nur noch eine Loprotine 
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            Unit otherUnit = other.gameObject.GetComponent<Unit>();
            if (otherUnit.Owner != Owner)
            {
                BehaviourScript.UpdateState();
                StartCoroutine(UpdateAI());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            Unit otherUnit = other.gameObject.GetComponent<Unit>();
            if (otherUnit.Owner != Owner)
            {
                BehaviourScript.UpdateState();
                StopCoroutine(UpdateAI());
            }
        }
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
        OnDeath.Invoke();
        OnDeath.RemoveAllListeners();
        
        if (SpawnedBy != null ) SpawnedBy.SpawnedUnits.Remove(gameObject);

        Destroy(gameObject);
    }
}