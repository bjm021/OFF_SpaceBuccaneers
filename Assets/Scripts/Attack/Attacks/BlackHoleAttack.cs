using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BlackHoleAttack : Attack
{
    private LineRenderer _lineRenderer;
    public override bool SpecificAttack(GameObject target)
    {
        // Run attack on server / host / singleplayer
        VisualAttackRender(gameObject.transform.position, target.transform.position);
        // Run attack on clients
        if (GameManager.Instance.Host) DrawOnClientRpc(gameObject.transform.position, target.transform.position);

        //var targetUnit = target.GetComponent<Unit>();
        
        var targets = Physics.OverlapSphere(target.transform.position, 5f, LayerMask.GetMask("Unit"));
        
        foreach (var t in targets)
        {
            if (t.TryGetComponent(out Mothership mothership))
            {
                mothership.TakeDamage(Damage);
            } 
            else
            {
                var unit = t.GetComponent<Unit>();
                
                if (TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce((t.transform.position - transform.position).normalized * 1000f, ForceMode.Impulse);
                }
                
                unit.TakeDamage(Damage);
            }
        }
        
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