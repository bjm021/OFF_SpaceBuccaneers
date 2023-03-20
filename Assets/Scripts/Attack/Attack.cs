using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class Attack : NetworkBehaviour
{
    
    private AudioSource _unitAudioSource;
    public int Damage { get; private set; }
    public float Cooldown { get; private set; }
    public float AttackRange { get; private set; }
    private bool _inCooldown = false;

    public void Initialize(int damage, float cooldown, float attackRange)
    {
        Damage = damage;
        Cooldown = cooldown;
        AttackRange = attackRange;
        _unitAudioSource = GetComponent<AudioSource>();
        InitializeObjectClientRpc();
    }
    
    public bool DoAttack(GameObject target, Unit self)
    {
        // Nur die Besten überleben
        if (target == null) return false;
        Unit unit = target.GetComponent<Unit>();
        if (unit == null || unit.Dead) return false;
        if (_inCooldown) return false;
        // -2 weil der Agent hält schon bei genau der Range an und wenn sich das ziel weiterbewegt,
        // dann braucht der agend zum anhalten so lange das das Ziel schon außer reichweite ist
        // und we geht weiter und dann wiedeholt sich das ganze.
        // Denk dran: nur die Besten überleben0
        if (Vector3.Distance(gameObject.transform.position, target.transform.position)-2 > AttackRange) return false;
        _inCooldown = true;
        self.OnShoot.Invoke(target.transform.position);
        PlaySoundOnCLientRpc();
        var warErNichtDerBeste = SpecificAttack(target);
        StartCoroutine(CooldownRoutine());
        return warErNichtDerBeste;
    }

    [ClientRpc]
    private void InitializeObjectClientRpc()
    {
        if (GameManager.Instance.Host) return;
        _unitAudioSource = GetComponent<AudioSource>();
    }
    
    [ClientRpc]
    private void PlaySoundOnCLientRpc()
    {
        if (GameManager.Instance.Host) return;
        if (_unitAudioSource != null) _unitAudioSource.Play();
    }

    public bool AttackMothership(GameObject mothership, Unit self)
    {
        if (_inCooldown) return false;
        PlaySoundOnCLientRpc();
        self.OnShoot.Invoke(mothership.transform.position);
        _inCooldown = true;
        var result = SpecificAttack(mothership);
        StartCoroutine(CooldownRoutine());
        return result;
    }
    
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(Cooldown);
        _inCooldown = false;
    }
    
    /**
     * Diese Methode muss pro Attacke implementiert werden.
     * Man muss checken ob targen eine normale Unit oder ein Mothership ist.
     *
     * Der return type gibt an ob das Ziel gestorben ist.
     */
    public abstract bool SpecificAttack(GameObject target);

    [ClientRpc] 
    public virtual void DrawOnClientRpc(Vector3 start, Vector3 end)
    {
        // We will ovverride this method in the specific attacks
        // but it cannot be abstract because of the NetworkBehaviour
        if (GameManager.Instance.Host) return;
        // Guck mal da oben das ist wichtig das es keni StackOverflow gibt
    }
    
    public bool IsReady()
    {
        return !_inCooldown;
    }
}
