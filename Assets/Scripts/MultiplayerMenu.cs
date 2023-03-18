using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private TMP_Text multiplayerButtonText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Toggle hostToggle;
    [SerializeField] private TMP_Text localIPText;

    private string _localIP;
    
    private void Awake()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hostEntry.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                _localIP = ip.ToString();
                break;
            }
        }
        
        localIPText.text = _localIP;
        inputField.text = _localIP;
        multiplayerButton.interactable = false;
    }

    public void MultiplayerButtonClicked()
    {
        if (hostToggle.isOn)
        {
            // Start host
        }
        else
        {
            // Join as client
            // Get IP from inputField.text
        }
    }
    
    public void HostToggleChanged(bool value)
    {
        if (value)
        {
            // Is host
            multiplayerButtonText.text = "Start host";

            if (inputField.text == "")
            {
                inputField.text = _localIP;
                multiplayerButton.interactable = true;
            }
            else
            {
                multiplayerButton.interactable = CheckIfIPIsValid(inputField.text);
            }
        }
        else
        {
            // Is client
            multiplayerButtonText.text = "join as Client";

            multiplayerButton.interactable = CheckIfIPIsValid(inputField.text);
        }
    }
    
    public void InputFieldChanged(string value)
    {
        if (CheckIfIPIsValid(value) || hostToggle.isOn) // TODO: Check if IP is valid
        {
            multiplayerButton.interactable = true;
        }
        else
        {
            multiplayerButton.interactable = false;
        }
    }

    private bool CheckIfIPIsValid(string ip)
    {
        // TODO: Check if a game is running on the IP
        return false;
    }
}
