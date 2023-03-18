using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AbilityManager : NetworkBehaviour
{
    [SerializeField] private List<AbilityClass> abilityClasses;
    [SerializeField] public bool multiplayerBehaviour = false;
    
    public List<AbilityClass> AbilityClasses => abilityClasses;

    #region Singleton

    public static AbilityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Debug.LogWarning("More than one instance of AbilityManager found!");
            Destroy(gameObject);
        }
    }

    #endregion

    public void Initialize()
    {
        
    }
    
    public void SpawnAbility(Vector3 position, int abilityIndex, GameManager.Player owner)
    {
        // Todo - Check cost and cooldown

        if (!GameManager.Instance.Host)
        {
            Debug.LogWarning("Launch ServerRpc as player " + owner);
            SpawnAbilityServerRpc(position, abilityIndex, (int) owner);
            return;
        }
        
        if (abilityIndex >= 6) abilityIndex = abilityIndex - 6;
        var abilityClass = abilityClasses[abilityIndex];
        var abilityGo = Instantiate(abilityClass.AbilityPrefab, position, Quaternion.identity);
        if (multiplayerBehaviour) abilityGo.GetComponent<NetworkObject>().Spawn();
        var ability = abilityGo.GetComponent<Ability>();
        ability.Initialize(abilityClass, owner, position);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnAbilityServerRpc(Vector3 position, int abilityIndex, int ownerIndex)
    {
        SpawnAbility(position, abilityIndex, (GameManager.Player)ownerIndex);
    }
    
    
}
