using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParticleAttack : Attack
{
    [SerializeField] private GameObject _shotVFX;
    [SerializeField] private AudioClip _chargeSound;
    [SerializeField] private AudioClip _fireSound;
    private AudioSource _audioSource;
    
    private Transform _target;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override bool SpecificAttack(GameObject target)
    {
        _audioSource.PlayOneShot(_chargeSound);
        StartCoroutine(AttackRoutine(target));
        return false;
    }

    public void DelayedAttack(GameObject target)
    {
        _target = target.transform;
        
        // Run attack on server / host / singleplayer
        VisualAttackRender(gameObject.transform.position, target.transform.position);
        // Run attack on clients
        if (GameManager.Instance.Host) DrawOnClientRpc(gameObject.transform.position, target.transform.position);

        //var targetUnit = target.GetComponent<Unit>();
        
        if (target.TryGetComponent(out Mothership mothership))
        {
            mothership.TakeDamage(Damage);
        } 
        else
        {
            var unit = target.GetComponent<Unit>();
            var self = gameObject.GetComponent<Unit>();
            if (self.UnitClass.name == "HackingHarpoon")
            {
                unit.Stun(2.5f);
            }
            unit.TakeDamage(Damage);
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
        GameObject shot = Instantiate(_shotVFX, start, rotation, _target);
        Destroy(shot, 10);
    }
    
    private IEnumerator AttackRoutine(GameObject target)
    { 
        _audioSource.PlayOneShot(_chargeSound);
        yield return new WaitForSeconds(1f);
        _audioSource.PlayOneShot(_fireSound);
        DelayedAttack(target);
    }
}
