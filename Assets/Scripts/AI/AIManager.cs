using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField]
    [Range(0.2f, 7)]
    [Tooltip("The intervall of the AI state update (Target recalculation among other stuff)")]

    private float updateInterval = 1f;
    
    #region Singleton
    
    public static AIManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of AsteroidManager found!");
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    private List<GameObject> units = new List<GameObject>();
    
    public void AddUnit(GameObject unit)
    {
        units.Add(unit);
    }
    
    public void RemoveUnit(GameObject unit)
    {
        units.Remove(unit);
    }
    
    public List<GameObject> GetUnits()
    {
        return units;
    }

    private Coroutine updateRoutine;
    
    private void Start()
    {
        updateRoutine = StartCoroutine(UpdateStateRoutine());
    }
    
    private IEnumerator UpdateStateRoutine()
    {
        while (true)
        {
            foreach (var unit in units)
            {
                unit.GetComponent<Unit>().BehaviourScript.UpdateState();
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
