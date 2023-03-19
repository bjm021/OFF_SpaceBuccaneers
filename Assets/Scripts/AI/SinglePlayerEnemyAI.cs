using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SinglePlayerEnemyAI : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private List<UnitClass> spawnableUnits;
    private float _x = 80f;


    private void Start()
    {
        if (!GameManager.Instance.Host) return;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            var unitClass = spawnableUnits[Random.Range(0, spawnableUnits.Count)];
            var pos = new Vector3(_x , 0, Random.Range(-40, 40));
            UnitManager.Instance.SpawnUnit(pos, unitClass, GameManager.Player.PlayerTwo, null);
        }
    }
    
}
