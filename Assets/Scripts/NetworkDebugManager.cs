using System.Collections;
using System.Collections.Generic;
using ParrelSync;
using Unity.Netcode;
using UnityEngine;

public class NetworkDebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject asteroidSpawners;
    private void Awake()
    {
        var networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Debug.Log("NetworkDebugManager started");
        if (ClonesManager.IsClone())
        {
            var argument = ClonesManager.GetArgument();
            Debug.Log("I am a " + argument);
            if (argument == "client")
            {
                GameManager.Instance.IsHost = false;
                networkManager.StartClient();
            }
        } 
        else 
        {
            GameManager.Instance.IsHost = true;
            Debug.Log("I am the server");
            networkManager.StartHost();
        }
    }
    
}
