using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : Ability
{
    public override void DoAttack(Vector3 start)
    {
        // spherecast to find all enemies in range
        var colliders = Physics.OverlapSphere(start, AbilityClass.EmpRange, LayerMask.GetMask("SpaceLaserLayer"));
        foreach (var c in colliders)
        {
            var unit = c.GetComponentInParent<Unit>();
            if (unit == null) continue;
            unit.Stun(AbilityClass.EmpStunTime);
        }
    }

    public override void DoAttackVisuals(Vector3 start = default)
    {
        // TODO implement
        throw new System.NotImplementedException();
    }
}
