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
    [Space]
    [SerializeField] private GameObject pauseMenu;
    [Space]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text winScreenText;

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

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void DisplayWinScreen(GameManager.Player winner)
    {
        winScreen.SetActive(true);
        
        switch (winner)
        {
            case GameManager.Player.PlayerOne:
                winScreenText.text = "Player One Won!";
                break;
            case GameManager.Player.PlayerTwo:
                winScreenText.text = "Player Two Won!";
                break;
        }
    }
}
