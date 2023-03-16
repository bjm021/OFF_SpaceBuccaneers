using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private UnitClass unitClass;
    [SerializeField] private GameManager.Player owner;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float spawnRadius = 1;
    [SerializeField] private float spawnDelay = 1;
    [SerializeField] private int spawnCount = 1;
    [SerializeField] private int absoluteCount = -1;
    [SerializeField] private int unitsOnScreenLimit = 10;
    [SerializeField] private bool multiplayerBehaviour = false;
    
    public List<GameObject> SpawnedUnits { get; } = new List<GameObject>();

    private void Start()
    {
        if (multiplayerBehaviour)
        {
            if (GameManager.Instance.IsHost && owner == GameManager.Player.PlayerTwo)
            {
                Destroy(gameObject);
                return;
            } else if (!GameManager.Instance.IsHost && owner == GameManager.Player.PlayerOne)
            {
                Destroy(gameObject);
                return;
            }
        }
        StartCoroutine(SpawnUnits());
    }
    
    private IEnumerator SpawnUnits()
    {
        var spawnCounter = 0;
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            
            for (int i = 0; i < spawnCount; i++)
            {
                if (SpawnedUnits.Count >= unitsOnScreenLimit) continue;
                if (absoluteCount != -1 && spawnCounter >= absoluteCount) yield break;
                var position = spawnPoint.Length > 0
                    ? spawnPoint[Random.Range(0, spawnPoint.Length)].position
                    : transform.position;
                position += Random.insideUnitSphere * spawnRadius;
                position = new Vector3(position.x, 0, position.z);
                UnitManager.Instance.SpawnUnit(position, unitClass, owner, this);
                spawnCounter += spawnCount;
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        switch (owner)
        {
            case GameManager.Player.PlayerOne:
                Gizmos.color = Color.blue;
                break;
            case GameManager.Player.PlayerTwo:
                Gizmos.color = Color.red;
                break;
        }
        
        foreach (var spawnPoints in spawnPoint)
        {
            Gizmos.DrawWireSphere(spawnPoints.position, spawnRadius + 0.1f);
        }
    }
}
