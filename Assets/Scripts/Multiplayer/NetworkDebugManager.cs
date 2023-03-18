using Unity.Netcode;
using UnityEngine;

public class NetworkDebugManager : MonoBehaviour
{
    NetworkManager networkManager;
    private bool _host;
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
        }
        else
        {
            _host = false;
            GameManager.Instance.Host = false;
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
