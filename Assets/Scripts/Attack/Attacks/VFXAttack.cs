using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VFXAttack : Attack
{
    [SerializeField] private GameObject _shotVFX;
    public override bool SpecificAttack(GameObject target)
    {
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
            return unit.TakeDamage(Damage) <= 0;
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
        Quaternion rotation = Quaternion.LookRotation(end - start);
        GameObject shot = Instantiate(_shotVFX, start, rotation);
        shot.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(end, start));
        Destroy(shot, 0.33f);
    }
}