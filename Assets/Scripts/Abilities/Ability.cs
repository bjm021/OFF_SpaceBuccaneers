using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

public abstract class Ability : NetworkBehaviour
{
    public AbilityClass AbilityClass { get; set; }

    public GameManager.Player Owner { get; set; }
    
    public void Initialize(AbilityClass abilityClass, GameManager.Player owner, Vector3 start)
    {
        if (!GameManager.Instance.Host) owner = GameManager.Player.PlayerTwo;
        AbilityClass = abilityClass;
        Owner = owner;
        DoAttack(start);
        DoAttackVisuals(start);
        if (GameManager.Instance.Host) DrawOnClientRpc(start);
    }
    public abstract void DoAttack(Vector3 start);
    
    public abstract void DoAttackVisuals(Vector3 start);
    
    [ClientRpc] 
    public void DrawOnClientRpc(Vector3 start)
    {
        if (GameManager.Instance.Host) return;
        Owner = GameManager.Player.PlayerTwo;
        Debug.LogWarning("ClientRPc  AS pLAYER " + Owner + " from " + start + "");
        DoAttackVisuals(start);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
 
}   
