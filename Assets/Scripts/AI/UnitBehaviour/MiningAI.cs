using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiningAI : MonoBehaviour, IAIBehaviour
{ 
    private enum MiningState { GoingTo, Mining, Waiting };
    private MiningState _state = MiningState.GoingTo;
    private NavMeshAgent _agent;
    private Unit _unit;

    private GameObject _currentasteroid = null;
    Asteroid _currentAsteroidManager = null;

    public void UpdateState()
    {
        if (_state == MiningState.GoingTo)
        {
            if (_agent.remainingDistance < 3)
            {
                _state = MiningState.Mining;
                // TODO - Start mining
                StartCoroutine(MiningCoroutine());
            }
        }
        else if (_state == MiningState.Mining)
        {
            // TODO - Check if mining is done currently done in MiningWaitCoroutine as a timer
            // TODO - If mining is done, go back to going to state
        }
        else if (_state == MiningState.Waiting)
        {
            FindAndGoToClosestAsteroid();
        }
    }
    
    private IEnumerator MiningCoroutine()
    {
        while (true)
        {
            var remaining = _currentAsteroidManager.Mine(_unit.UnitClass.MiningRate);
            // TODO - ADD RESOURCES TO INVENTORY
            Debug.Log("Resources left " + remaining);
            if (remaining <= 0 || _currentAsteroidManager.Dead)
            {
                _state = MiningState.Waiting;
                yield break;
            }
            yield return new WaitForSeconds(_unit.UnitClass.MiningTimeUnitLength);
        }
    }
    
    
    void IAIBehaviour.Start()
    {
        _unit = GetComponent<Unit>();
        _agent = GetComponent<NavMeshAgent>();
        FindAndGoToClosestAsteroid();
        _state = MiningState.GoingTo;
    } 

    private void FindAndGoToClosestAsteroid()
    {
        _state = MiningState.GoingTo;
        GameObject tMin = null;
        var minDist = Mathf.Infinity;
        
        foreach (var asteroid in AsteroidManager.Instance.Asteroids)
        {
            if (_currentasteroid != null && _currentasteroid == asteroid.gameObject) continue;
            
            var dist = Vector3.Distance(asteroid.gameObject.transform.position, gameObject.transform.position);
            if (!(dist < minDist)) continue;
            tMin = asteroid.gameObject;
            minDist = dist;
        }

        if (tMin == null) return;
        _currentasteroid = tMin;
        _currentAsteroidManager = tMin.GetComponent<Asteroid>();
        _agent.SetDestination(tMin.transform.position);
    }
}
