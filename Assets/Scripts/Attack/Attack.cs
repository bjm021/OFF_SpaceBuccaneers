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
    
    public bool DoAttack(GameObject target)
    {
        if (target == null) return false;
        Unit unit = target.GetComponent<Unit>();
        if (unit == null || unit.Dead) return false;
        if (_inCooldown) return false;
        // -2 weil der Agent hält schon bei genau der Range an und wenn sich das ziel weiterbewegt,
        // dann braucht der agend zum anhalten so lange das das Ziel schon außer reichweite ist
        // und we geht weiter und dann wiedeholt sich das ganze.
        if (Vector3.Distance(gameObject.transform.position, target.transform.position)-2 > AttackRange) return false;
        _inCooldown = true;
        var result = SpecificAttack(target);
        StartCoroutine(CooldownRoutine());
        return result;
    }
    
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(Cooldown);
        _inCooldown = false;
    }
    
    public abstract bool SpecificAttack(GameObject target);
    
    public bool IsReady()
    {
        return !_inCooldown;
    }
}
