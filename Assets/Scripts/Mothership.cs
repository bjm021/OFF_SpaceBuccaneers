using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour
{
    [SerializeField] public GameManager.Player owner;
    [SerializeField] private int maxHealth;
    [SerializeField] private UnitClass mothershipObject;

    public int CurrentHealth { get; private set; }
    
    private void Awake()  
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

    private void Start()
    {
        var unit = GetComponent<Unit>();
        unit.Owner = owner;
        unit.UnitClass = mothershipObject;
        unit.BehaviourScript = new StandStillAI();
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
