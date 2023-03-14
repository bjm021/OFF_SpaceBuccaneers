using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiningAI : MonoBehaviour, IAIBehaviour
{ 
    private enum MiningState { GoingTo, Mining, Waiting };
    
    private MiningState _state = MiningState.GoingTo;
    
    private NavMeshAgent _agent;

    private GameObject _currentMeteor = null;

    public void UpdateState()
    {
        if (_state == MiningState.GoingTo)
        {
            if (_agent.remainingDistance < 1)
            {
                _state = MiningState.Mining;
                // TODO - Start mining
                StartCoroutine(MiningWaitCoroutine());
            }
        }
        else if (_state == MiningState.Mining)
        {
            // TODO - Check if mining is done currently done in MiningWaitCoroutine as a timer
            // TODO - If mining is done, go back to going to state
        }
        else if (_state == MiningState.Waiting)
        {
            FindAndGoToClosestMeteor();
        }
    }
    
    private IEnumerator MiningWaitCoroutine()
    {
        // TODO - Wait for mining to be done
        yield return new WaitForSeconds(3);
        _state = MiningState.Waiting;
    }
    
    
    void IAIBehaviour.Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        FindAndGoToClosestMeteor();
        _state = MiningState.GoingTo;
    } 

    private void FindAndGoToClosestMeteor()
    {
        _state = MiningState.GoingTo;
        GameObject tMin = null;
        var minDist = Mathf.Infinity;
        
        foreach (var asteroid in AsteroidManager.Instance.Asteroids)
        {
            if (_currentMeteor != null && _currentMeteor == asteroid.gameObject) continue;
            
            var dist = Vector3.Distance(asteroid.gameObject.transform.position, gameObject.transform.position);
            if (!(dist < minDist)) continue;
            tMin = asteroid.gameObject;
            minDist = dist;
        }

        if (tMin == null) return;
        _currentMeteor = tMin;
        _agent.SetDestination(tMin.transform.position);
    }
}
