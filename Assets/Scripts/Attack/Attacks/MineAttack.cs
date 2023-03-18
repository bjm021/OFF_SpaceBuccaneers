using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MineAttack : Attack
{
    private void Start()
    {
        Debug.LogWarning("MineAttack init ");
    }

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
        var unit = GetComponent<Unit>();
        var colliders = Physics.OverlapSphere(gameObject.transform.position, unit.UnitClass.AttackRange, LayerMask.NameToLayer("Unit"));
        foreach (var collider in colliders)
        {
            var unitInRadius = collider.GetComponent<Unit>();
            if (unitInRadius != null)
            {
                unitInRadius.TakeDamage(unit.UnitClass.Attack);
            }
        }
        unit.TakeDamage(int.MaxValue);
    }
}