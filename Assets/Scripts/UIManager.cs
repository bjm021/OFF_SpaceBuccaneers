using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    
    public static UIManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of UIManager found!");
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    [SerializeField] private TMP_Text timeText;
    [Space]
    [SerializeField] private TMP_Text playerOneMetalText;
    [SerializeField] private TMP_Text playerOneCrystalText;
    [SerializeField] private TMP_Text playerTwoMetalText;
    [SerializeField] private TMP_Text playerTwoCrystalText;

    public void UpdateTimeText(int time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("0");
        string seconds = (time % 60).ToString("00");
        
        timeText.text = minutes + ":" + seconds;
    }
    
    public void UpdateResourceText(GameManager.Player player, GameManager.ResourceType resourceType, int amount)
    {
        switch (player)
        {
            case GameManager.Player.PlayerOne:
                switch (resourceType)
                {
                    case GameManager.ResourceType.Metal:
                        playerOneMetalText.text = amount.ToString();
                        break;
                    case GameManager.ResourceType.Crystals:
                        playerOneCrystalText.text = amount.ToString();
                        break;
                }
                break;
            case GameManager.Player.PlayerTwo:
                switch (resourceType)
                {
                    case GameManager.ResourceType.Metal:
                        playerTwoMetalText.text = amount.ToString();
                        break;
                    case GameManager.ResourceType.Crystals:
                        playerTwoCrystalText.text = amount.ToString();
                        break;
                }
                break;
        }
    }
}
