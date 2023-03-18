using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class MineFieldAbility : Ability
{
    public override void DoAttack(Vector3 start)
    {
        Debug.LogWarning("MININIMINIMINMINMM");
        for (int i = 0; i < AbilityClass.MineCount; i++)
        {
            
            var position = new Vector3(Random.Range(AbilityClass.MineMinLowerCoordinate, AbilityClass.MineMaxUpperCoordinate), 0, Random.Range(AbilityClass.MineMinLowerCoordinate, AbilityClass.MineMaxUpperCoordinate));
            Debug.LogWarning("Spawning mine at " + position + " for player " + GameManager.Player.PlayerOne);
            //var asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], position, 
            //    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            UnitManager.Instance.SpawnUnit(position, AbilityClass.MineUnitClass, GameManager.Player.PlayerOne);

            //if (multiplayerBehaviour) asteroid.GetComponent<NetworkObject>().Spawn(); 


        }
    }
    
    public override void DoAttackVisuals(Vector3 start)
    {
        // TODO implement
        throw new System.NotImplementedException();
    }
}