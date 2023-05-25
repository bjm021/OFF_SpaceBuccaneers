using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    
    [SerializeField] private PlayerController playerController;
    [Space]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Material[] timeTextMaterials;
    [Space]
    [SerializeField] private TMP_Text playerOneMetalText;
    [SerializeField] private TMP_Text playerOneCrystalText;
    [SerializeField] private TMP_Text playerTwoMetalText;
    [SerializeField] private TMP_Text playerTwoCrystalText;
    [Space]
    [SerializeField] private Image playerOneHealthBar;
    [SerializeField] private Image playerTwoHealthBar;
    [Space]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseSettingsMenu;
    [Space]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text winScreenText;
    [Space]
    [SerializeField] private GameObject spawnableAreaIndicator;
    [SerializeField] private GameObject playerOneRedArea;
    [SerializeField] private GameObject playerTwoRedArea;
    [Space]
    [SerializeField] private Image[] cooldownImages;
    
    public void ShowSpawnableAreaIndicator(float width, GameManager.Player player, bool active = true)
    {
        if (player == GameManager.Player.PlayerOne)
        {
            playerOneRedArea.SetActive(active);
        }
        else
        {
            playerTwoRedArea.SetActive(active);
        }
        spawnableAreaIndicator.SetActive(active);
    }
    
    public void UpdateTimeText(int time, bool recolor = false)
    {
        string minutes = Mathf.Floor(time / 60).ToString("0");
        string seconds = (time % 60).ToString("00");
        
        timeText.text = minutes + ":" + seconds;
        timeText.fontSharedMaterial = timeTextMaterials[recolor ? 1 : 0];
    }
    
    public void UpdateResourceText()
    {
        playerOneMetalText.text = GameManager.Instance.PlayerOneMetal.ToString();
        playerOneCrystalText.text = GameManager.Instance.PlayerOneCrystals.ToString();
        playerTwoMetalText.text = GameManager.Instance.PlayerTwoMetal.ToString();
        playerTwoCrystalText.text = GameManager.Instance.PlayerTwoCrystals.ToString();
    }
    
    public void UpdateMotherShipHealth(GameManager.Player player, float health)
    {
        switch (player)
        {
            case GameManager.Player.PlayerOne:
                playerOneHealthBar.fillAmount = health;
                break;
            case GameManager.Player.PlayerTwo:
                playerTwoHealthBar.fillAmount = health;
                break;
        }
    }
    
    public void UpdateCooldownImages(float cooldown)
    {
        foreach (var cooldownImage in cooldownImages)
        {
            cooldownImage.fillAmount = cooldown;
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            playerController.enabled = false;
            if (!GameManager.Instance.inMultiplayerMode) Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            pauseSettingsMenu.SetActive(false);
            playerController.enabled = true;
            if (!GameManager.Instance.inMultiplayerMode) Time.timeScale = 1;
        }
    }

    public void DisplayWinScreen(GameManager.Player winner)
    {
        winScreen.SetActive(true);
        
        switch (winner)
        {
            case GameManager.Player.PlayerOne:
                winScreenText.text = "Blue Won!";
                break;
            case GameManager.Player.PlayerTwo:
                winScreenText.text = "Red Won!";
                break;
        }
    }
}
