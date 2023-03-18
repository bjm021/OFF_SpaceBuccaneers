using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLaser : Ability
{
    public override void DoAttack(Vector3 start)
    {
        Transform realStart;
        if (Owner == GameManager.Player.PlayerOne)
        {
            realStart = GameManager.Instance.PlayerOneMothership.transform.GetChild(0);
        }
        else
        {
            realStart = GameManager.Instance.PlayerOneMothership.transform.GetChild(0);
        }
        
        
        var direction = start-realStart.position;
        RaycastHit[] hits = Physics.SphereCastAll(realStart.position, AbilityClass.SpaceLaserWidth/2, direction.normalized * 200, 200, LayerMask.GetMask("SpaceLaserLayer"));
        
        Debug.DrawRay(start, Vector3.up*200, Color.red, 5);
        
        Debug.DrawRay(realStart.position, direction.normalized * 200, Color.red, 5);
        
        foreach (var hit in hits)
        {
            hit.collider.GetComponentInParent<Unit>().TakeDamage(int.MaxValue);
        }
        
        var beam = gameObject.AddComponent<LineRenderer>();
        beam.startColor = Color.red;
        beam.endColor = Color.red;
        beam.startWidth = 0.1f;
        beam.endWidth = 0.1f;
        beam.startWidth = AbilityClass.SpaceLaserWidth;
        beam.endWidth = AbilityClass.SpaceLaserWidth;
        beam.positionCount = 2;
        beam.SetPosition(0, realStart.position);
        beam.SetPosition(1, realStart.position + direction.normalized * 200);
        //beam.material = new Material(Shader.Find("Sprites/Default"));
        beam.useWorldSpace = true;
        StartCoroutine(DestroyBeam(beam));
    } 
    
    private IEnumerator DestroyBeam(LineRenderer beam)
    {
        yield return new WaitForSeconds(1f);
        Destroy(beam);
    }

    public override void DoAttackVisuals(Vector3 start = default)
    {
        // TODO implement
        throw new System.NotImplementedException();
    }
}
