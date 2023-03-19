using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : NetworkBehaviour
{
    [SerializeField] private Material playerOneMaterial;
    [SerializeField] private Material playerTwoMaterial;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private int[] materialIndices;
    
    public UnitClass UnitClass { get; set; }
    public GameManager.Player Owner { get; set; }
    public IAIBehaviour BehaviourScript { get; set; }
    public bool Dead { get; private set; }
    public UnitSpawner SpawnedBy { get; private set; } = null;
    
    public UnityEvent OnDeath = new();
    public UnityEvent<Vector3> OnShoot = new();
    
    private int _currentHealth;
    private NavMeshAgent _navMeshAgent;
    private Attack _attack;
    private SphereCollider _viewTrigger;
    public bool Stunned { get; set; } = false;

    private Coroutine _updateAI;

    public void Initialize(UnitClass unitClass, GameManager.Player owner, UnitSpawner spawnedBy)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attack = GetComponent<Attack>();
        _viewTrigger = GetComponent<SphereCollider>();
        
        UnitClass = unitClass;
        Owner = owner;
        Dead = false;
        SpawnedBy = spawnedBy;

        _currentHealth = UnitClass.Health;
        
        _navMeshAgent.speed = UnitClass.MoveSpeed;
        _navMeshAgent.stoppingDistance = UnitClass.AttackRange - 1;
        
        _viewTrigger.radius = UnitClass.AttackSeekRange;

        for (var index = 0; index < renderers.Length; index++)
        {
            var materials = renderers[index].materials;
            materials[materialIndices[index]] = Owner == GameManager.Player.PlayerOne ? playerOneMaterial : playerTwoMaterial;
            renderers[index].materials = materials;
        }

        _attack.Initialize(UnitClass.Attack, UnitClass.AttackCooldown, UnitClass.AttackRange);

        BehaviourScript = UnitClass.behaviour switch
        {
            UnitClass.AIBehaviourType.Passive => gameObject.AddComponent<PassiveAI>(),
            UnitClass.AIBehaviourType.Mining => gameObject.AddComponent<MiningAI>(),
            UnitClass.AIBehaviourType.Aggressive => gameObject.AddComponent<AggressiveAI>(),
            UnitClass.AIBehaviourType.StandStill => gameObject.AddComponent<StandStillAI>(),
            UnitClass.AIBehaviourType.SpecialMining => gameObject.AddComponent<SpecialMiningAI>(),
            UnitClass.AIBehaviourType.SpecialPrioritisingAggressive => gameObject.AddComponent<SpecialPrioritisingAggressiveAI>(),
            _ => BehaviourScript 
        };

        if (GameManager.Instance.Host) ColorUpdateOnClientRpc((int) Owner);
        BehaviourScript.Start();
    }

    private IEnumerator UpdateAI()
    {
        while (true)
        {
            BehaviourScript.UpdateState();
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Stunned) return;
        // TODO - Nur noch eine Loprotine 
        if (!GameManager.Instance.Host) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit") || other.gameObject.layer == LayerMask.NameToLayer("Mothership"))
        {
            Unit otherUnit = other.gameObject.GetComponent<Unit>();
            if (otherUnit.Owner != Owner)
            {
                BehaviourScript.UpdateState();
                
                if (_updateAI == null)
                {
                    _updateAI = StartCoroutine(UpdateAI());
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!GameManager.Instance.Host) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit") || other.gameObject.layer == LayerMask.NameToLayer("Mothership"))
        {
            Unit otherUnit = other.gameObject.GetComponent<Unit>();
            if (otherUnit.Owner != Owner)
            {
                BehaviourScript.UpdateState();
                
                // TODO: Throws a NullReferenceException sometimesif (!Physics.CheckSphere(transform.position, _viewTrigger.radius, 1 << LayerMask.NameToLayer("Unit")))
                {
                    StopCoroutine(UpdateAI());
                    _updateAI = null;
                }
            }
        }
    }

    public int TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        return _currentHealth;
    }

    private void Die()
    {
        Dead = true;
        OnDeath.Invoke();
        if (GameManager.Instance.Host) KillOnClientRpc();
        OnDeath.RemoveAllListeners();
        
        gameObject.layer = LayerMask.NameToLayer("Default");
		transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");        
		if (_navMeshAgent != null)
        {
            _navMeshAgent.enabled = false;
            _viewTrigger.enabled = false;
        }

        if (_updateAI != null)
        {
            StopCoroutine(_updateAI);
            _updateAI = null;
        }
        if (SpawnedBy != null ) SpawnedBy.SpawnedUnits.Remove(gameObject);

        // TODO: Adjust despawn time
        if (!GameManager.Instance.Host) return;
        var delay = 5f;
        StartCoroutine(DelayedDestroy(delay));
    }
    
    public IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    
    private Coroutine _stunCoroutine;

    private IEnumerator StunCoroutine(float d)
    {
        yield return new WaitForSeconds(d);
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        Stunned = false;
    }
    
    public void Stun(float duration)
    {
        if (Stunned) return;
        bool wasAIOn = _updateAI != null;
        if (wasAIOn)
        {
            StopCoroutine(_updateAI);
            _updateAI = null;
        }
        Stunned = true;
        if (_navMeshAgent != null && _navMeshAgent.enabled)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }

        if (_stunCoroutine != null) StopCoroutine(_stunCoroutine);
        _stunCoroutine = StartCoroutine(StunCoroutine(duration));
        if (wasAIOn)
        {
            _updateAI = StartCoroutine(UpdateAI());
        }
    }

    [ClientRpc]
    private void ColorUpdateOnClientRpc(int playerIndex)
    {
        if (GameManager.Instance.Host) return;
        for (var index = 0; index < renderers.Length; index++)
        {
            var materials = renderers[index].materials;
            GameManager.Player o = (GameManager.Player)playerIndex;
            materials[materialIndices[index]] = o == GameManager.Player.PlayerOne ? playerOneMaterial : playerTwoMaterial;
            renderers[index].materials = materials;
        }
    }
    
    [ClientRpc]
    private void KillOnClientRpc()
    {
        if (GameManager.Instance.Host) return;
        Debug.LogWarning("KillOnClientRpc");
        Die();
    }
}