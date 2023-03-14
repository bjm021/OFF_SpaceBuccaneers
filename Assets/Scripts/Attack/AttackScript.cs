using System.Collections;
using UnityEngine;

public abstract class AttackScript : MonoBehaviour
{
    public float Cooldown { get; private set; }
    public float AttackRange { get; private set; }
    private bool _inCooldown = false;

    public void Initialize(float cooldown, float attackRange)
    {
        Cooldown = cooldown;
        AttackRange = attackRange;
    }
    
    public void Attack(GameObject target)
    {
        if (_inCooldown) return;
        if (Vector3.Distance(gameObject.transform.position, target.transform.position) > AttackRange) return;
        _inCooldown = true;
        SpecificAttack(target);
        StartCoroutine(CooldownRoutine());
    }
    
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(Cooldown);
        _inCooldown = false;
    }
    
    public abstract bool SpecificAttack(GameObject target);
}
