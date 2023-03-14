using UnityEngine;

public class Unit : ScriptableObject
{
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed;
    
    
    
    [SerializeField] private int attack;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    
    public int Health => health;
    public float MoveSpeed => moveSpeed;
    public int Attack => attack;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
}
