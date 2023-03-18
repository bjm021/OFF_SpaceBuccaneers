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
    
    public bool SpawnAbility(Vector3 position, AbilityClass abilityClass, GameManager.Player owner)
    {
        switch (owner)
        {
            case GameManager.Player.PlayerOne when GameManager.Instance.PlayerOneMetal < abilityClass.Cost:
                return false;
            case GameManager.Player.PlayerOne:
                GameManager.Instance.RemoveResource(GameManager.Player.PlayerOne, GameManager.ResourceType.Crystals, abilityClass.Cost);
                break;
            case GameManager.Player.PlayerTwo when GameManager.Instance.PlayerTwoMetal < abilityClass.Cost:
                return false;
            case GameManager.Player.PlayerTwo:
                GameManager.Instance.RemoveResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Crystals, abilityClass.Cost);
                break;
        }
        
        if (!GameManager.Instance.Host)
        {
            SpawnAbilityServerRpc(position, Instance.AbilityClasses.IndexOf(abilityClass), (int) owner);
            return true;
        }
        
        var abilityGo = Instantiate(abilityClass.AbilityPrefab, position, Quaternion.identity);
        if (multiplayerBehaviour) abilityGo.GetComponent<NetworkObject>().Spawn();
        var ability = abilityGo.GetComponent<Ability>();
        ability.Initialize(abilityClass, owner, position);
        return true;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnAbilityServerRpc(Vector3 position, int abilityIndex, int ownerIndex)
    {
        SpawnAbility(position, abilityClasses[abilityIndex], (GameManager.Player)ownerIndex);
    }
    
    
}
