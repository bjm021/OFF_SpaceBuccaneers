using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAttack : Attack
{
    public override bool SpecificAttack(GameObject target)
    {
        return false;
    }
}
