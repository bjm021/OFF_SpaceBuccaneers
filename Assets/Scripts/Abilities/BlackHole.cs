using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Ability
{
    Collider[] affectedUnits;
   List<Rigidbody> affectedRbs = new List<Rigidbody>();
    private bool _updateForce = false;
    private Vector3 _start;

    public override void DoAttack(Vector3 start)
    {
        _start = start;
        affectedUnits = Physics.OverlapSphere(start, AbilityClass.BlackHoleRange, LayerMask.GetMask("Unit"));
        for (var i = 0; i < affectedUnits.Length; i++)
        {
            var unitRb = affectedUnits[i].GetComponent<Rigidbody>();
            if (unitRb == null) continue;
            unitRb.AddForce((start - affectedUnits[i].transform.position).normalized * AbilityClass.BlackHolePullForce,
                ForceMode.Acceleration);
            affectedRbs.Add(unitRb);
        }

        StartCoroutine(BlackHoleRoutine(start));
        _updateForce = true;
    }

    private IEnumerator BlackHoleRoutine(Vector3 start) 
    {
        affectedRbs.Clear();
        for (int i = 0; i < AbilityClass.BlackHoleDuration; i++)
        {
            affectedUnits = Physics.OverlapSphere(start, AbilityClass.BlackHoleRange, LayerMask.GetMask("Unit"));
            for (var i1 = 0; i1 < affectedUnits.Length; i1++)
            {
                affectedRbs.Add(affectedUnits[i1].GetComponent<Rigidbody>());
            }

            yield return new WaitForSeconds(1);
        }
        affectedRbs.Clear();
        _updateForce = false;
        affectedRbs = null;
        affectedUnits = null;
        Die();
    }

    private void FixedUpdate()
    {
        if (!_updateForce) return;
        foreach (var affectedRb in affectedRbs)
        {
            affectedRb.AddForce((_start - affectedRb.transform.position).normalized * AbilityClass.BlackHolePullForce,
                ForceMode.Acceleration);
        }
    }

    public override void DoAttackVisuals(Vector3 start)
    {
        // TODO implement
        throw new System.NotImplementedException();
    }
}