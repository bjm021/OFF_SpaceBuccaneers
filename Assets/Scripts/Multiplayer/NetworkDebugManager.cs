using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkDebugManager : NetworkBehaviour
{
    NetworkManager _networkManager;
    private bool _host;
    private UnityTransport _utp;
    
    [SerializeField] private GameObject _waitingPanel;
    [SerializeField] private TMP_Text _waitingText;
    
    private void Awake()
    {
        _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        _utp = _networkManager.GetComponent<UnityTransport>();
        var networkDataCarrier = FindObjectOfType<NetworkDataCarrier>();
        if (networkDataCarrier == null)
        {
            Debug.LogError("No NetworkDataCarrier found");
            return;
        }
        
        
        if (networkDataCarrier.Host)
        {
            _host = true;
            GameManager.Instance.Host = true;
            _waitingText.text = "Waiting for client";
            _utp.ConnectionData.ServerListenAddress = networkDataCarrier.IP;
            _utp.ConnectionData.Port = (ushort)networkDataCarrier.Port;
            _utp.ConnectionData.Address = networkDataCarrier.IP;
        }
        else
        {
            _host = false;
            _waitingText.text = "Connecting to server";
            GameManager.Instance.Host = false;
            _utp.ConnectionData.Address = networkDataCarrier.IP;
            _utp.ConnectionData.Port = (ushort)networkDataCarrier.Port;
        }
    }


    private void Start()
    {
        if (_host)
        {
            _networkManager.StartHost();
            _waitingPanel.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Waiting for client, starting routine");
            StartCoroutine(ServerClientJoined());
        }
        else
        {
            _waitingPanel.SetActive(true);
            _networkManager.StartClient();
            StartCoroutine(ClientConnection());
        }
        
    }

    private IEnumerator ClientConnection()
    {
        while (!_networkManager.IsConnectedClient)
        {
            yield return null;
        }
        Debug.Log("Client joined, starting game");
        StartCoroutine(CountDown());
    }
    
    private IEnumerator ServerClientJoined()
    {
        while (_networkManager.ConnectedClients.Count < 2)
        {
            yield return null;
        }
        Debug.Log("Client joined, starting game");
        StartCoroutine(CountDown());
    }
    
    private IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            _waitingText.text = "Starting in: " + i;
            yield return new WaitForSecondsRealtime(1);
        }
        _waitingPanel.SetActive(false);
        Time.timeScale = 1;
    }

   
}
