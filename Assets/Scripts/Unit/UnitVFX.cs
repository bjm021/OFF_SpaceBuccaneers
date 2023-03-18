using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitVFX : MonoBehaviour
{
    [SerializeField] private UnityEvent OnShoot = new UnityEvent();
    [SerializeField] UnityEvent OnDeath = new UnityEvent();
    
    public void Shoot(Vector3 target)
    {
        OnShoot.Invoke();
    }
    
    public void Death()
    {
        OnDeath.Invoke();
    }
    
    public void StartParticleSystem(ParticleSystem particle)
    {
        particle.Play();
    }
    
    public void StartVFX(GameObject vfx)
    {
        vfx.SetActive(true);
    }
    
    public void SetVFXTrigger(GameObject vfx, string trigger)
    {
        vfx.GetComponent<Animator>().SetTrigger(trigger);
    }
}
