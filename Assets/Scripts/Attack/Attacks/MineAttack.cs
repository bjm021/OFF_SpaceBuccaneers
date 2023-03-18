using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MineAttack : Attack
{

    public override bool SpecificAttack(GameObject target)
    {
        return false;
    }

    [ClientRpc]
    public override void DrawOnClientRpc(Vector3 start, Vector3 end)
    {
        if (GameManager.Instance.Host) return;
        VisualAttackRender(start, end);
    }

    private void VisualAttackRender(Vector3 start, Vector3 end)
    {
        // TODO - EXPLOSION BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM 
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("MineAttack OnTriggerEnter from " + other.gameObject.name);
        var unit = GetComponent<Unit>();
        
        Debug.LogWarning("Cast sphere with range " + unit.UnitClass.AttackRange + " and layer " + LayerMask.NameToLayer("SpaceLaserLayer") + "");
        var colliders = Physics.OverlapSphere(gameObject.transform.position, unit.UnitClass.AttackRange, LayerMask.NameToLayer("SpaceLaserLayer"));
        foreach (var c in colliders)
        {
            Debug.LogWarning("Hit from " + other.name);
            var otherUnit = other.GetComponentInParent<Unit>();
            Debug.LogWarning("Other unti is class " + otherUnit.UnitClass.name);
            Debug.LogWarning("Dealing " + unit.UnitClass.Attack + " damage to " + otherUnit.UnitClass.name + "");
            otherUnit.TakeDamage(unit.UnitClass.Attack);
        }
        unit.TakeDamage(int.MaxValue);
    }
}