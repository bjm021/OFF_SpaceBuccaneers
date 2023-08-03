using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SinglePlayerEnemyAI : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private List<UnitClass> spawnableUnits;
    [SerializeField] private GameManager.Player player;
    [SerializeField] private float SpawnerX = 80f;


    private void OnEnable()
    {
        if (!GameManager.Instance.Host) return;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            var unitClass = spawnableUnits[Random.Range(0, spawnableUnits.Count)];
            var pos = new Vector3(SpawnerX , 0, Random.Range(-40, 40));
            UnitManager.Instance.SpawnUnit(pos, unitClass, player, null);
        }
    }
    
    public void SetInterval(int interval)
    {
        _spawnInterval = interval;
    }
    
}
