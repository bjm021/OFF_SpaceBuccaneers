using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviourScript : MonoBehaviour
{

    [SerializeField] private ScriptableObject ai;

    public IAIBehaviour behaviourScript;

    private void Start()
    {
        if (ai is UnitAI unitAI)
        {
            behaviourScript = unitAI.behaviour switch
            {
                AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
                AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
                AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
                AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
                _ => behaviourScript 
            };
        }

        AIManager.Instance.AddUnit(gameObject);
        behaviourScript.Start();
    }
}
