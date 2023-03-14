using UnityEngine;

public enum AIBehaviourType
{
    Passive, Aggressive, Mining, StandStill
}

[CreateAssetMenu(fileName = "UnitAI", menuName = "UnitAI", order = 0)]
public class UnitAI : ScriptableObject
{
    [SerializeField]
    public AIBehaviourType behaviour;
    
}
