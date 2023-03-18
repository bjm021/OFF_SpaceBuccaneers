using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HarpoonAttack : Attack
{
    private LineRenderer _lineRenderer;
    public override bool SpecificAttack(GameObject target)
    {
        Debug.Log("Harpoon Attack");
        // Run attack on server / host / singleplayer
        VisualAttackRender(gameObject.transform.position, target.transform.position);
        // Run attack on clients
        if (GameManager.Instance.Host) DrawOnClientRpc(gameObject.transform.position, target.transform.position);

        //var targetUnit = target.GetComponent<Unit>();
        
        if (target.TryGetComponent(out Mothership mothership))
        {
            return mothership.TakeDamage(Damage) <= 0;
        } 
        else
        {
            var unit = target.GetComponent<Unit>();
            var dead = unit.TakeDamage(Damage) <= 0;
            
            // TODO - Implement stun
            if (!dead)
            {
                Debug.Log("Stun");
                unit.Stun(2.5f);
            }
            
            gameObject.GetComponent<SpecialPrioritisingAggressiveAI>().SearchNewTarget();

            return dead;
        }
        
        
        
        
        
        
       
        
    }

    [ClientRpc]
    public override void DrawOnClientRpc(Vector3 start, Vector3 end)
    {
        if (GameManager.Instance.Host) return;
        VisualAttackRender(start, end);
    }

    private void VisualAttackRender(Vector3 start, Vector3 end)
    {
        var beam = gameObject.AddComponent<LineRenderer>();
        beam.startColor = Color.red;
        beam.endColor = Color.red;
        beam.startWidth = 0.1f;
        beam.endWidth = 0.1f;
        beam.positionCount = 2;
        beam.SetPosition(0, start);
        beam.SetPosition(1, end);
        //beam.material = new Material(Shader.Find("Sprites/Default"));
        beam.useWorldSpace = true;
        StartCoroutine(DestroyBeam(beam));
    }
    

    private IEnumerator DestroyBeam(LineRenderer beam)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(beam);
    }
}
