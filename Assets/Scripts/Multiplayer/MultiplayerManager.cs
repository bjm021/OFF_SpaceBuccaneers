using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    
    [SerializeField] private GameObject asteroidSpawners;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("Starting MultiplayerManager as " + (GameManager.Instance.Host ? "host" : "client") + "...");
        if (!GameManager.Instance.Host)
        {
            Debug.Log("Disabling asteroid spawners...");
        }
    }
    
}
