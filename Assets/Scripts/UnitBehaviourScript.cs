using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviourScript : MonoBehaviour
{

    [SerializeField] private ScriptableObject ai;

    private IAIBehaviour behaviourScript;

    private void Start()
    {
        if (ai is UnitAI unitAI)
        {
            behaviourScript = unitAI.behaviour switch
            {
                AIBehaviourType.PASSIVE => gameObject.AddComponent<PassiveAI>(),
                _ => behaviourScript
            };
        }

        behaviourScript.Start();
    }
}
