using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitOwner
    {
        Player,
        Enemy
    }

    [SerializeField] private UnitClass unitClass;
    [SerializeField] private UnitOwner owner;
    
    public UnitClass UnitClass => unitClass;
    public UnitOwner Owner => owner;
    
    private int _currentHealth;
    
    public IAIBehaviour behaviourScript;


    private void Awake()
    {
        _currentHealth = unitClass.Health;
    }

    private void Start()
    {
        behaviourScript = unitClass.behaviour switch
        {
            UnitClass.AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
            UnitClass.AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
            UnitClass.AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
            UnitClass.AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
            _ => behaviourScript 
        };
        AIManager.Instance.AddUnit(gameObject);
        behaviourScript.Start();
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