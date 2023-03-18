using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BlackHoleAttack : Attack
{
    private LineRenderer _lineRenderer;

    public override bool SpecificAttack(GameObject target)
    {
        // do nothing
        return false;


        /*
        Debug.LogWarning("BlackHoleAttack");
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
        
        */
    }

    [ClientRpc]
    public override void DrawOnClientRpc(Vector3 start, Vector3 end)
    {
        // Do nothing
    }

    private void VisualAttackRender(Vector3 start, Vector3 end)
    {
        // Do nothing
    }

    private Coroutine _attackCoroutine;

    private void Start()
    {
        _attackCoroutine = StartCoroutine(AttackCoroutine(GetComponent<Unit>()));
    }


    private IEnumerator AttackCoroutine(Unit self)
    {


        while (true)
        {
            // Run attack on server / host / singleplayer
            //VisualAttackRender(gameObject.transform.position, target.transform.position);
            // Run attack on clients
            //if (GameManager.Instance.Host) DrawOnClientRpc(gameObject.transform.position, target.transform.position);

            //var targetUnit = target.GetComponent<Unit>();

            if (self.Dead) yield break;
            var targets = Physics.OverlapSphere(gameObject.transform.position, self.UnitClass.AttackRange,
                LayerMask.GetMask("SpaceLaserLayer", "Mothership"));

            foreach (var t in targets)
            {
                var parent = t.transform.parent;
                if (parent.gameObject == gameObject) continue;
                if (parent.TryGetComponent(out Mothership mothership))
                {
                    mothership.TakeDamage(Damage);
                }
                else
                {
                    var unit = parent.GetComponent<Unit>();

                    if (parent.TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddForce((transform.position - parent.transform.position).normalized * 10f, ForceMode.Impulse);
                    }

                    unit.TakeDamage(Damage);
                }
            }

            yield return new WaitForSeconds(0.25f);

            /*
        var colliders = Physics.OverlapSphere(transform.position, self.UnitClass.AttackRange, LayerMask.GetMask("Unit"));
        foreach (var c in colliders)
        {
            Debug.LogWarning("Colliding with " + c.name);
            if (c.TryGetComponent(out Mothership mothership))
            {
                mothership.TakeDamage(Damage);
            } 
            else
            { 
                Debug.LogWarning("Dealing " + Damage + " damage to " + c.name);
                var unit = c.GetComponent<Unit>();
                unit.TakeDamage(Damage);
            }
        }

        yield return new WaitForSeconds(0.25f);
    }
    */
        }
    }
}