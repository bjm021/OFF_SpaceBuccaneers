using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpaceLaser : Ability
{
    [SerializeField] private float beamTime = 1f;
    
    public override void DoAttack(Vector3 start)
    {
        Transform realStart;
        if (Owner == GameManager.Player.PlayerOne)
        {
            realStart = GameManager.Instance.PlayerOneMothership.transform.GetChild(0);
        }
        else
        {
            realStart = GameManager.Instance.PlayerTwoMothership.transform.GetChild(0);
        }
        
        
        var direction = start-realStart.position;
        RaycastHit[] hits = Physics.SphereCastAll(realStart.position, AbilityClass.SpaceLaserWidth/2, direction.normalized * 200, 200, LayerMask.GetMask("SpaceLaserLayer"));
        
        Debug.DrawRay(start, Vector3.up*200, Color.red, 5);
        
        Debug.DrawRay(realStart.position, direction.normalized * 200, Color.red, 5);
        
        foreach (var hit in hits)
        {
            hit.collider.GetComponentInParent<Unit>().TakeDamage(int.MaxValue);
        }
    } 
    
    private IEnumerator DestroyBeam(LineRenderer beam)
    {
        yield return new WaitForSeconds(1f);
        Destroy(beam);
        Die();
    }

    public override void DoAttackVisuals(Vector3 start = default)
    {
        Vector3 _laserPosition;
        
        if (Owner == GameManager.Player.PlayerOne)
        {
            _laserPosition = GameManager.Instance.PlayerOneMothership.transform.GetChild(0).position;
        }
        else
        {
            _laserPosition = GameManager.Instance.PlayerTwoMothership.transform.GetChild(0).position;
        }
        
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.y = 0;
            
        transform.position = _laserPosition;
        transform.LookAt(worldPosition);
        
        Destroy(gameObject, beamTime);
    }
}
