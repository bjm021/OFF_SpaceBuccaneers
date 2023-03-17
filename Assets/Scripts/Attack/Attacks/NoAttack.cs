using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NoAttack : Attack
{
    public override bool SpecificAttack(GameObject target)
    {
        return false;
    }

    [ClientRpc]
    public override void DrawOnClientRpc(Vector3 start, Vector3 end)
    {
        // DO NOTHING
    }
}
