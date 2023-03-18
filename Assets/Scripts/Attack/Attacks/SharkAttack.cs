using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SharkAttack : Attack
{
    private LineRenderer _lineRenderer;
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Mothership mothership)) return;
        mothership.TakeDamage(Damage);
        GetComponent<Unit>().TakeDamage(int.MaxValue);
    }
}