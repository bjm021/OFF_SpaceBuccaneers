using System;
using System.Collections;
using System.Collections.Generic;
using ParrelSync;
using Unity.Netcode;
using UnityEngine;

public class NetworkDebugManager : MonoBehaviour
{
    NetworkManager networkManager;
    // Start is called before the first frame update
    [SerializeField] GameObject asteroidSpawners;
    private void Awake()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Debug.Log("NetworkDebugManager started");
        if (ClonesManager.IsClone())
        {
            var argument = ClonesManager.GetArgument();
            Debug.Log("I am a " + argument);
            if (argument == "client")
            {
                GameManager.Instance.IsHost = false;
            }
        } 
        else 
        {
            GameManager.Instance.IsHost = true;
            Debug.Log("I am the server");
        }
    }


    private void Start()
    {
        if (ClonesManager.IsClone())
        {
            var argument = ClonesManager.GetArgument();
            Debug.Log("I am a " + argument);
            if (argument == "client")
            {
                networkManager.StartClient();
            }
        }
        else
        {
            networkManager.StartHost();
        }
    }
}
