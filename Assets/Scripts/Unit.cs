using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitOwner
    {
        Player,
        Enemy
    }

    public IAIBehaviour BehaviourScript;
    public UnitClass UnitClass { get; set; }
    public UnitOwner Owner { get; set; }

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = UnitClass.Health;
        
        BehaviourScript = UnitClass.behaviour switch
        {
            UnitClass.AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
            UnitClass.AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
            UnitClass.AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
            UnitClass.AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
            _ => BehaviourScript 
        };
        AIManager.Instance.AddUnit(gameObject);
        BehaviourScript.Start();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}