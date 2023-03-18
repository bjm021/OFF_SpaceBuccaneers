using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkDebugManager : MonoBehaviour
{
    NetworkManager networkManager;
    private bool _host;
    private UnityTransport _utp;
    private void Awake()
    {
        var networkDataCarrier = FindObjectOfType<NetworkDataCarrier>();
        if (networkDataCarrier == null)
        {
            Debug.LogError("No NetworkDataCarrier found");
            return;
        }
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        
        if (networkDataCarrier.Host)
        {
            _host = true;
            GameManager.Instance.Host = true;
            _utp.ConnectionData.ServerListenAddress = networkDataCarrier.IP;
            _utp.ConnectionData.Port = (ushort)networkDataCarrier.Port;
            _utp.ConnectionData.Address = networkDataCarrier.IP;
        }
        else
        {
            _host = false;
            GameManager.Instance.Host = false;
            _utp.ConnectionData.Address = networkDataCarrier.IP;
            _utp.ConnectionData.Port = (ushort)networkDataCarrier.Port;
        }
    }


    private void Start()
    {
        if (_host)
        {
            networkManager.StartHost();
        }
        else
        {
            networkManager.StartClient();
        }
    }
}
