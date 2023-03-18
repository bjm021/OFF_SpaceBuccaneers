using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Ability : NetworkBehaviour
{
    public AbilityClass AbilityClass { get; set; }

    public GameManager.Player Owner { get; set; }
    
    public void Initialize(AbilityClass abilityClass, GameManager.Player owner, Vector3 start)
    {
        AbilityClass = abilityClass;
        Owner = owner;
        DoAttack(start);
        DoAttackVisuals(start);
    }
    public abstract void DoAttack(Vector3 start);
    
    public abstract void DoAttackVisuals(Vector3 start);
    
    [ClientRpc] 
    public virtual void DrawOnClientRpc(Vector3 start)
    {
        if (GameManager.Instance.Host) return;
        DoAttackVisuals(start);
    }

}
