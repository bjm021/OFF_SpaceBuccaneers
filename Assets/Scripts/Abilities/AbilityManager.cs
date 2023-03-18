using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<AbilityClass> abilityClasses;
    
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
        if (abilityIndex >= 6) abilityIndex = abilityIndex - 6;
        var abilityClass = abilityClasses[abilityIndex];
        var abilityGo = Instantiate(abilityClass.AbilityPrefab, position, Quaternion.identity);
        var ability = abilityGo.GetComponent<Ability>();
        ability.Initialize(abilityClass, owner, position);
    }
    
    
}
