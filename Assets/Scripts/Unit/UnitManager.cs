using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitManager : NetworkBehaviour
{
    #region Singleton

    public static UnitManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Debug.LogWarning("More than one instance of UnitManager found!");
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private List<UnitClass> unitClasses = new List<UnitClass>();
    [SerializeField] private bool multiplayerBehaviour = false;
    public List<UnitClass> UnitClasses => unitClasses;

    private void Initialize() 
    {
        // Do nothing
    }

    // Wird nur vom Client aufgerufen
    [ServerRpc(RequireOwnership = false)]
    public void SpawnUnitServerRpc(Vector3 position, int unitIndex)
    {
        UnitClass unitClass = UnitManager.Instance.UnitClasses[unitIndex];
        GameManager.Instance.RemoveResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Metal, unitClass.Cost);
        var unitGo = Instantiate(unitClass.UnitPrefab, position, Quaternion.Euler(0, -90, 0));
        if (multiplayerBehaviour) unitGo.GetComponent<NetworkObject>().Spawn();
        var unit = unitGo.GetComponent<Unit>();
        unit.Initialize(unitClass, GameManager.Player.PlayerTwo, null);
    }

    public bool SpawnUnit(Vector3 position, UnitClass unitClass, GameManager.Player owner, UnitSpawner spawnedBy = null)
    {
        Debug.LogWarning("TEsting for " + owner + " to has " + unitClass.Cost + " metal");
        switch (owner)
        {
            case GameManager.Player.PlayerOne when GameManager.Instance.PlayerOneMetal < unitClass.Cost:
                return false;
            case GameManager.Player.PlayerOne:
                GameManager.Instance.RemoveResource(GameManager.Player.PlayerOne, GameManager.ResourceType.Metal, unitClass.Cost);
                break;
            case GameManager.Player.PlayerTwo when GameManager.Instance.PlayerTwoMetal < unitClass.Cost:
                return false;
            case GameManager.Player.PlayerTwo:
                GameManager.Instance.RemoveResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Metal, unitClass.Cost);
                break;
        }
        
        if (!GameManager.Instance.Host)
        {
            SpawnUnitServerRpc(position, unitClasses.IndexOf(unitClass));
            return true;
        }

        var unitGo = Instantiate(unitClass.UnitPrefab, position, Quaternion.Euler(0, 90, 0));
        if (multiplayerBehaviour) unitGo.GetComponent<NetworkObject>().Spawn();
        if (spawnedBy != null)
        {
            spawnedBy.SpawnedUnits.Add(unitGo);
        }
        var unit = unitGo.GetComponent<Unit>();

        unit.Initialize(unitClass, owner, spawnedBy);
        return true;
        
    }
}