using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIBehaviourType
{
    PASSIVE, AGGRESSIVE, MINING
}

[CreateAssetMenu(fileName = "UnitAI", menuName = "UnitAI", order = 0)]
public class UnitAI : ScriptableObject
{
    [SerializeField]
    public AIBehaviourType behaviour;
    

}
