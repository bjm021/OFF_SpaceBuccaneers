using System.ComponentModel;
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
    [SerializeField] private float attackSeekRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] public AIBehaviourType behaviour;
    [Tooltip("The number of Resources mined per Time Units")] [SerializeField] public int miningRate;
    [Tooltip("The number of seconds a mining time unit should take")][SerializeField] private float miningTimeUnitLength = 1;
    [SerializeField] private float miningRange = 2.5f;
    
    public GameObject UnitPrefab => unitPrefab;
    public int Cost => cost;
    public int Health => health;
    public float MoveSpeed => moveSpeed;
    public int Attack => attack;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public int MiningRate => miningRate;
    public float MiningTimeUnitLength => miningTimeUnitLength = 10f;
    public float AttackSeekRange => attackSeekRange;

    public float MiningRange => miningRange;
    
    public enum AIBehaviourType
    {
        Passive, Aggressive, Mining, StandStill, SpecialMining
    }
}
