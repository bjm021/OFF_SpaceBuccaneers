using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
    
    [SerializeField] [Range(90, 300)] private int roundLength;
    [Space]
    [SerializeField] private int metalAutoGenerationAmount;
    [SerializeField] private int metalAutoGenerationInterval;

    private bool _isInRound;
    
    private int _playerOneMetal;
    private int _playerOneCrystals;
    
    private int _playerTwoMetal;
    private int _playerTwoCrystals;
    
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
        
        UIManager.Instance.UpdateResourceText(player, resourceType, amount);
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
        
        UIManager.Instance.UpdateResourceText(player, resourceType, amount);
    }
    
    public void Start()
    {
        StartRound();
    }
    
    public void StartRound()
    {
        _isInRound = true;
        StartCoroutine(Round());
        StartCoroutine(MetalAutoGeneration());
    }
    
    public void EndRound()
    {
        _isInRound = false;
    }

    public void MothershipDestroyed(Mothership mothership)
    {
        if (mothership == PlayerOneMothership)
        {
            Debug.Log("Player 2 wins!");
        }
        else if (mothership == PlayerTwoMothership)
        {
            Debug.Log("Player 1 wins!");
        }
    }

    private IEnumerator Round()
    {
        int time = roundLength;
        UIManager.Instance.UpdateTimeText(time);
        
        while (time > 60)
        {
            yield return new WaitForSeconds(1);
            time--;
            UIManager.Instance.UpdateTimeText(time);
        }

        // TODO: Resource generation is doubled
        
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            UIManager.Instance.UpdateTimeText(time);
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
    
    public GameObject GetEnemyMothership(Unit.UnitOwner player)
    {
        switch (player)
        {
            case Unit.UnitOwner.PlayerOne:
                return PlayerTwoMothership.gameObject;
            case Unit.UnitOwner.PlayerTwo:
                return PlayerOneMothership.gameObject;
            default:
                return null;
        }
    }

    public void EndGame(Unit.UnitOwner losingPlayer)
    {
        Unit.UnitOwner winningPlayer;
        switch (losingPlayer)
        {
            case Unit.UnitOwner.PlayerOne:
                winningPlayer = Unit.UnitOwner.PlayerTwo;
                break;
            case Unit.UnitOwner.PlayerTwo:
                winningPlayer = Unit.UnitOwner.PlayerOne;
                break;
        }
        
        AIManager.Instance.StopAI();
        Time.timeScale = 0;

        // TODO - Something like UIManager.Instance.ShowEndGameScreen(winningPlayer);
    }
}