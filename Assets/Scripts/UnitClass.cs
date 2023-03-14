using UnityEngine;

[CreateAssetMenu(fileName = "UnitClass", menuName = "UnitClass", order = 0)]
public class UnitClass : ScriptableObject
{
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private int cost;
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int attack;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] public AIBehaviourType behaviour;
    
    public GameObject UnitPrefab => unitPrefab;
    public int Cost => cost;
    public int Health => health;
    public float MoveSpeed => moveSpeed;
    public int Attack => attack;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    
    public enum AIBehaviourType
    {
        Passive, Aggressive, Mining, StandStill
    }
}
