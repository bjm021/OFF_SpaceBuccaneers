using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour
{
    [SerializeField] private Unit.UnitOwner owner;
    [SerializeField] private int maxHealth;

    private int _currentHealth;
    
    private void Awake()
    {
        switch (owner)
        {
            case Unit.UnitOwner.PlayerOne:
                GameManager.Instance.PlayerOneMothership = this;
                break;
            case Unit.UnitOwner.PlayerTwo:
                GameManager.Instance.PlayerTwoMothership = this;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _currentHealth = maxHealth;
    }
    
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        GameManager.Instance.MothershipDestroyed(this);
        // TODO: Add death animation
    }
}
