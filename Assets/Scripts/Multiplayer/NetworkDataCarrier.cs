using System;
using System.Net;
using UnityEngine;


public class NetworkDataCarrier : MonoBehaviour
{
    public string IP { get; set; }
    public int Port { get; set; } = 7777;
    public bool Host { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
