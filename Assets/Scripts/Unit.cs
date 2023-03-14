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

    private void Awake()
    {
        _currentHealth = unitClass.Health;
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
