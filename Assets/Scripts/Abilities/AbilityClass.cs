using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityClass", menuName = "AbilityClass", order = 0)]
public class AbilityClass : ScriptableObject
{
    [SerializeField] private GameObject abilityPrefab;
    [SerializeField] private int cost;
    [SerializeField] private float cooldown;
    [Header("EMP")]
    [SerializeField] private float empRange;
    [SerializeField] private float empStunTime;
    [Header("Black Hole")]
    [SerializeField] private float blackHoleRange;
    [SerializeField] private float blackHolePullForce;
    [SerializeField] private float blackHoleDuration;
    [Header("Spaaaaace Laaaaaser")]
    [SerializeField] private float spaceLaserWidth;
    [Header("MineField")]
    [SerializeField] private float mineCount;
    [SerializeField] private float mineRadius;
    [SerializeField] private UnitClass mineUnitClass;

    public GameObject AbilityPrefab => abilityPrefab;
    public int Cost => cost;
    public float Cooldown => cooldown;
    public float EmpRange => empRange;
    public float EmpStunTime => empStunTime;
    public float BlackHoleRange => blackHoleRange;
    public float BlackHolePullForce => blackHolePullForce;
    public float BlackHoleDuration => blackHoleDuration;
    public float SpaceLaserWidth => spaceLaserWidth;
    public float MineCount => mineCount;
    public float MineRadius => mineRadius;
    public UnitClass MineUnitClass => mineUnitClass;
    
}
