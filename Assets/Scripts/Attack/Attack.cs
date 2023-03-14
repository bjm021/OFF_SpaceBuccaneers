using System.Collections;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public int Damage { get; private set; }
    public float Cooldown { get; private set; }
    public float AttackRange { get; private set; }
    private bool _inCooldown = false;

    public void Initialize(int damage, float cooldown, float attackRange)
    {
        Damage = damage;
        Cooldown = cooldown;
        AttackRange = attackRange;
    }
    
    public void DoAttack(GameObject target)
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
