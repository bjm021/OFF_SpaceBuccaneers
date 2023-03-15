using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
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
    public List<UnitClass> UnitClasses => unitClasses;

    private void Initialize()
    {
        // Do nothing
    }

    public bool SpawnUnit(Vector3 position, UnitClass unitClass, Unit.UnitOwner owner, UnitSpawner spawnedBy = null)
    {
        {
            // TODO: Check if player has enough resources to spawn unit, if not return false
            // Also remove resources from player

            var unitGo = Instantiate(unitClass.UnitPrefab, position, Quaternion.identity);
            if (spawnedBy != null)
            {
                spawnedBy.SpawnedUnits.Add(unitGo);
            }
            var unit = unitGo.GetComponent<Unit>();

            unit.Initialize(unitClass, owner, spawnedBy);
            return true;
        }
    }
}
