using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour
{
    [SerializeField] private GameManager.Player owner;
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    
    private void Start()  
    {
        switch (owner)
        {
            case GameManager.Player.PlayerOne:
                GameManager.Instance.PlayerOneMothership = this;
                break;
            case GameManager.Player.PlayerTwo:
                GameManager.Instance.PlayerTwoMothership = this;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        CurrentHealth = maxHealth;
    }
    
    public int TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Die();
        }

        return CurrentHealth;
    }
    
    private void Die()
    {
        GameManager.Instance.MothershipDestroyed(this);
        // TODO: Add death animation
    }
}
