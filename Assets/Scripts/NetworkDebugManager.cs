using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkDebugManager : MonoBehaviour
{
    private bool _isHost = false;
    private String ip;
    private int port;
    private UnityTransport utp;
    NetworkManager networkManager;
    // Start is called before the first frame update
    private void Awake()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Debug.Log("NetworkDebugManager started");
        utp = networkManager.GetComponent<UnityTransport>();

        foreach (var commandLineArg in System.Environment.GetCommandLineArgs())
        {
            if (commandLineArg.StartsWith("--buccaIP="))
            {
                ip = commandLineArg.Substring(10);
            } 
            else if (commandLineArg.StartsWith("--buccaPort="))
            {
                port = int.Parse(commandLineArg.Substring(12));
            }
            else if (commandLineArg == "--buccaClient")
            {
                Debug.Log("I am a client");
                GameManager.Instance.Host = false;
                _isHost = false;
                utp.ConnectionData.Address = ip;
                utp.ConnectionData.Port = (ushort)port;
            }
            else if (commandLineArg == "--buccaServer")
            {
                Debug.Log("I am the server");
                GameManager.Instance.Host = true;
                _isHost = true;
                utp.ConnectionData.ServerListenAddress = ip;
                utp.ConnectionData.Port = (ushort)port;
                utp.ConnectionData.Address = ip;
            }
        }
        
        
        /*
        if (ClonesManager.IsClone())
        {
            var argument = ClonesManager.GetArgument();
            Debug.Log("I am a " + argument);
            if (argument == "client")
            {
                GameManager.Instance.Host = false;
            }
        } 
        else 
        {
            GameManager.Instance.Host = true;
            Debug.Log("I am the server");
        }
        */
    }


    private void Start()
    {
        /*
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
        */
        if (_isHost)
        {
            networkManager.StartHost();
        }
        else
        {
            networkManager.StartClient();
        }
    }
}
