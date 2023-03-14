using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private UnitClass unitClass;
    [SerializeField] private Unit.UnitOwner owner;
    [FormerlySerializedAs("spawnPositions")] [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float spawnRadius = 1;
    [SerializeField] private float spawnDelay = 1;
    [SerializeField] private int spawnCount = 1;
    [SerializeField] private int absoluteCount = -1;
    
    private void Start()
    {
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
                if (absoluteCount != -1 && spawnCounter >= absoluteCount) yield break;
                var position = spawnPoint.Length > 0
                    ? spawnPoint[Random.Range(0, spawnPoint.Length)].position
                    : transform.position;
                position += Random.insideUnitSphere * spawnRadius;
                position = new Vector3(position.x, 0, position.z);
                UnitManager.Instance.SpawnUnit(position, unitClass, owner);
                spawnCounter += spawnCount;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var spawnPoints in spawnPoint)
        {
            Gizmos.DrawWireSphere(spawnPoints.position, spawnRadius + 0.1f);
        }
    }
}
