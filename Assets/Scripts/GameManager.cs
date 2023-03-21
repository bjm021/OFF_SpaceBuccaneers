using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{ 
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            Destroy(gameObject);
        }
    }

    #endregion
    
    [SerializeField] private UnityEvent onRoundOver = new UnityEvent();
    [SerializeField] [Range(1, 900)] private int roundLength;
    [Space]
    [SerializeField] private int metalStartAmount;
    [SerializeField] private int crystalsStartAmount;
    [SerializeField] private int metalAutoGenerationAmount;
    [SerializeField] private int metalAutoGenerationInterval;
    [Space]
    [SerializeField] private GameObject playerOneExplosion;
    [SerializeField] private GameObject playerTwoExplosion;
    public bool inMultiplayerMode = false; 

    private bool _isInRound;
    
    private int _playerOneMetal; 
    private int _playerOneCrystals;
    
    private int _playerTwoMetal;
    private int _playerTwoCrystals;
    
    private int _time;

    public bool Host { get; set; } = true;
    
    public enum Player
    {
        PlayerOne, PlayerTwo
    }
    
    public enum ResourceType
    {
        Metal, Crystals
    }
    
    public Mothership PlayerOneMothership { get; set; }
    public Mothership PlayerTwoMothership { get; set; }
    
    public int PlayerOneMetal { get; private set; }
    public int PlayerOneCrystals { get; private set; }
    public int PlayerTwoMetal { get; private set; }
    public int PlayerTwoCrystals { get; private set; }
    
    public void AddResource(Player player, ResourceType resourceType, int amount)
    {
        switch (player)
        {
            case Player.PlayerOne:
                switch (resourceType)
                {
                    case ResourceType.Metal:
                        PlayerOneMetal += amount;
                        break;
                    case ResourceType.Crystals:
                        PlayerOneCrystals += amount;
                        break;
                }
                break;
            case Player.PlayerTwo:
                switch (resourceType)
                {
                    case ResourceType.Metal:
                        PlayerTwoMetal += amount;
                        break;
                    case ResourceType.Crystals:
                        PlayerTwoCrystals += amount;
                        break;
                }
                break;
        }

        if (Host && inMultiplayerMode)
            UpdateDataClientRpc(PlayerOneMetal, PlayerOneCrystals, PlayerTwoMetal, PlayerTwoCrystals, _time, PlayerOneMothership.CurrentHealth, PlayerTwoMothership.CurrentHealth);
        UIManager.Instance.UpdateResourceText();
    }

    [ClientRpc]
    private void UpdateDataClientRpc(int p1Metal, int p1Crystal, int p2Metal, int p2Crystal, int time, int p1mshp, int p2mshp)
    {
        if (Host) return;
        
        PlayerOneMetal = p1Metal;
        PlayerOneCrystals = p1Crystal;
        PlayerTwoMetal = p2Metal;
        PlayerTwoCrystals = p2Crystal;

        PlayerOneMothership.CurrentHealth = p1mshp;
        PlayerTwoMothership.CurrentHealth = p2mshp;


        UIManager.Instance.UpdateMotherShipHealth(Player.PlayerOne, (float )p1mshp / PlayerOneMothership.maxHealth);
        UIManager.Instance.UpdateMotherShipHealth(Player.PlayerTwo, (float )p2mshp / PlayerOneMothership.maxHealth);
        UIManager.Instance.UpdateResourceText();
        UIManager.Instance.UpdateTimeText(time);
    }
    
    public void RemoveResource(Player player, ResourceType resourceType, int amount)
    {
        switch (player)
        {
            case Player.PlayerOne:
                switch (resourceType)
                {
                    case ResourceType.Metal:
                        PlayerOneMetal -= amount;
                        break;
                    case ResourceType.Crystals:
                        PlayerOneCrystals -= amount;
                        break;
                }
                break;
            case Player.PlayerTwo:
                switch (resourceType)
                {
                    case ResourceType.Metal:
                        PlayerTwoMetal -= amount;
                        break;
                    case ResourceType.Crystals:
                        PlayerTwoCrystals -= amount;
                        break;
                }
                break;
        }
        if (Host && inMultiplayerMode) UpdateDataClientRpc(PlayerOneMetal, PlayerOneCrystals, PlayerTwoMetal, PlayerTwoCrystals, _time, PlayerOneMothership.CurrentHealth, PlayerTwoMothership.CurrentHealth);
        UIManager.Instance.UpdateResourceText();
    }
    
    public int GetResource(Player player, ResourceType resourceType)
    {
        switch (player)
        {
            case Player.PlayerOne:
                switch (resourceType)
                {
                    case ResourceType.Metal:
                        return PlayerOneMetal;
                    case ResourceType.Crystals:
                        return PlayerOneCrystals;
                }
                break;
            case Player.PlayerTwo:
                switch (resourceType)
                {
                    case ResourceType.Metal:
                        return PlayerTwoMetal;
                    case ResourceType.Crystals:
                        return PlayerTwoCrystals;
                }
                break;
        }

        return 0;
    }
    
    public void Start()
    {
        StartRound();
    }
    
    public void StartRound()
    {
        AddResource(Player.PlayerOne, ResourceType.Metal, metalStartAmount);
        AddResource(Player.PlayerOne, ResourceType.Crystals, crystalsStartAmount);
        AddResource(Player.PlayerTwo, ResourceType.Metal, metalStartAmount);
        AddResource(Player.PlayerTwo, ResourceType.Crystals, crystalsStartAmount);

        _isInRound = true;
        if (!Host) return;
        StartCoroutine(Round());
        StartCoroutine(MetalAutoGeneration());
    }
    
    public void EndRound()
    {
        _isInRound = false;

        if (PlayerOneMothership.CurrentHealth > PlayerTwoMothership.CurrentHealth)
        {
            EndGame(Player.PlayerOne);
        }
        else if (PlayerOneMothership.CurrentHealth < PlayerTwoMothership.CurrentHealth)
        {
            EndGame(Player.PlayerTwo);
        }
        else if (PlayerOneMothership.CurrentHealth == PlayerTwoMothership.CurrentHealth)
        {
            if (Random.Range(0, 2) == 0)
            {
                EndGame(Player.PlayerOne);
            }
            else
            {
                EndGame(Player.PlayerTwo);
            }
        }
    }

    public void MothershipDestroyed(Mothership mothership)
    {
        if (mothership == PlayerOneMothership)
        {
            EndGame(Player.PlayerTwo);
        }
        else if (mothership == PlayerTwoMothership)
        {
            EndGame(Player.PlayerOne);
        }
    }

    private IEnumerator Round()
    {
        _time = roundLength;
        UIManager.Instance.UpdateTimeText(_time);
        
        while (_time > 60)
        {
            yield return new WaitForSeconds(1);
            _time--;
            UIManager.Instance.UpdateTimeText(_time);
        }
        
        metalAutoGenerationAmount *= 2;

        while (_time > 0)
        {
            yield return new WaitForSeconds(1);
            _time--;
            UIManager.Instance.UpdateTimeText(_time, _time % 2 == 0);
        }
        
        EndRound();
    }
    
    private IEnumerator MetalAutoGeneration()
    {
        while (_isInRound)
        {
            yield return new WaitForSeconds(metalAutoGenerationInterval);
            AddResource(Player.PlayerOne, ResourceType.Metal, metalAutoGenerationAmount);
            AddResource(Player.PlayerTwo, ResourceType.Metal, metalAutoGenerationAmount);
        }
    }
    
    public GameObject GetEnemyMothership(Player player)
    {
        switch (player)
        {   
            case Player.PlayerOne:
                return PlayerTwoMothership.gameObject;
            case Player.PlayerTwo:
                return PlayerOneMothership.gameObject;
            default:
                return null;
        }
    }

    public void EndGame(Player winningPlayer)
    {
        switch (winningPlayer)
        {
            case Player.PlayerOne:
                playerTwoExplosion.SetActive(true);
                break;
            case Player.PlayerTwo:
                playerOneExplosion.SetActive(true);
                break;
        }
        
        if (Host && inMultiplayerMode)
        {
            RpcEndGameClientRpc((int) winningPlayer);
        }
        
        onRoundOver.Invoke();
        UIManager.Instance.DisplayWinScreen(winningPlayer);
        StartCoroutine(PauseTimeAfterDelay());
    }
    
    private IEnumerator PauseTimeAfterDelay()
    {
        float time = 1.25f;
        while (time > 0)
        {
            Time.timeScale = Mathf.Lerp(1, 0, 1 - time);
            yield return new WaitForSecondsRealtime(0.01f);
            time -= 0.01f;
        }
        Time.timeScale = 0;
    }

    
    [ServerRpc(RequireOwnership = false)]
    public void RpcEndGameServerRpc()
    {
        List<ulong> clientIDs = new List<ulong>();
        Debug.LogWarning("Disconnect all clients");
        foreach (var singletonConnectedClient in NetworkManager.Singleton.ConnectedClients)
        {
            Debug.LogWarning("Disconnected " + singletonConnectedClient.Key);
            clientIDs.Add(singletonConnectedClient.Key);
        }
        foreach (var clientID in clientIDs)
        {
            NetworkManager.Singleton.DisconnectClient(clientID);
        }
        Debug.LogWarning("DONE");
        NetworkManager.Singleton.Shutdown();
    }
        
    [ClientRpc]
    private void RpcEndGameClientRpc(int playerIndex)
    { 
        if (IsHost) return;
        EndGame(playerIndex);
    }

    private void EndGame(int player)
    {
        EndGame((Player) player);
    }
}