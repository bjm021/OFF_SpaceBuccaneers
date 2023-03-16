using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShoot : Attack
{
    private LineRenderer _lineRenderer;
    public override bool SpecificAttack(GameObject target)
    {
        var beam = gameObject.AddComponent<LineRenderer>();
        beam.startColor = Color.red;
        beam.endColor = Color.red;
        beam.startWidth = 0.1f;
        beam.endWidth = 0.1f;
        beam.positionCount = 2;
        beam.SetPosition(0, gameObject.transform.position);
        beam.SetPosition(1, target.transform.position);
        //beam.material = new Material(Shader.Find("Sprites/Default"));
        beam.useWorldSpace = true;
        StartCoroutine(DestroyBeam(beam));
        

        //var targetUnit = target.GetComponent<Unit>();
        
        if (target.TryGetComponent(out Mothership mothership))
        {
            return mothership.TakeDamage(Damage) <= 0;
        } 
        else
        {
            var unit = target.GetComponent<Unit>();
            return unit.TakeDamage(Damage) <= 0;
        }
        
       
        
    }
    
    private IEnumerator DestroyBeam(LineRenderer beam)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(beam);
    }
}
