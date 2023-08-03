using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject p1AI;
    [SerializeField] private GameObject p2AI;
    [SerializeField] private List<GameObject> notHostDisableElements;
    [SerializeField] private TMP_InputField p1AIInterval;
    [SerializeField] private TMP_InputField p2AIInterval;
    
    private SinglePlayerEnemyAI p1AIComponent;
    private SinglePlayerEnemyAI p2AIComponent;

    private void Awake()
    {
        
        
        if (!GameManager.Instance.Host)
        {
            notHostDisableElements.ForEach(button =>
            {
                button.GetComponent<Button>().interactable = false;
            });
        }
        
        p1AIComponent = p1AI.GetComponent<SinglePlayerEnemyAI>();
        p2AIComponent = p2AI.GetComponent<SinglePlayerEnemyAI>();
 
        p1AIInterval.text = "2";
        p2AIInterval.text = "2";
        p1AIInterval.onValueChanged.AddListener(delegate { p1AIComponent.SetInterval(Convert.ToInt32(p1AIInterval.text)); });
        p2AIInterval.onValueChanged.AddListener(delegate { p2AIComponent.SetInterval(Convert.ToInt32(p2AIInterval.text)); });
        
    }

    public void ToggleAI(int player)
    {
        switch (player) {
            case 0:
                p1AI.SetActive(!p1AI.activeSelf);
                break;
            case 1:
                p2AI.SetActive(!p2AI.activeSelf);
                break;
        }
    }

    public void AddP1Metal(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerOne, GameManager.ResourceType.Metal, amount);
        AddResourcesOnServerRpc((int) GameManager.Player.PlayerOne, (int) GameManager.ResourceType.Metal, amount);
    }
    
    public void AddP1Crystal(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerOne, GameManager.ResourceType.Crystals, amount);
        AddResourcesOnServerRpc((int) GameManager.Player.PlayerOne, (int) GameManager.ResourceType.Crystals, amount);
    }
    
    public void AddP2Metal(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Metal, amount);
        AddResourcesOnServerRpc((int) GameManager.Player.PlayerTwo, (int) GameManager.ResourceType.Metal, amount);
    }
    
    public void AddP2Crystal(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Crystals, amount);
        AddResourcesOnServerRpc((int) GameManager.Player.PlayerTwo, (int) GameManager.ResourceType.Crystals, amount);
    }
    
    public void AddP1MetalSP(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerOne, GameManager.ResourceType.Metal, amount);
    }
    
    public void AddP1CrystalSP(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerOne, GameManager.ResourceType.Crystals, amount);
    }
    
    public void AddP2MetalSP(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Metal, amount);
    }
    
    public void AddP2CrystalSP(int amount)
    {
        GameManager.Instance.AddResource(GameManager.Player.PlayerTwo, GameManager.ResourceType.Crystals, amount);
    }
    
    
    [ServerRpc (RequireOwnership = false)]
    private void AddResourcesOnServerRpc(int player, int resource, int amount)
    {
        Debug.LogError("SERVER RPC ADD RESOURCES");
        GameManager.Instance.AddResource((GameManager.Player) player, (GameManager.ResourceType) resource, amount);
    }
}
