using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool isMainMenu;
    [Space]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle pirateModeToggle;
    [SerializeField] private GameObject pirateModeText;
    [Space]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip pirateAudioClip;
    [Space]
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] private bool manageScale;
    
    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;
    private bool _pirateMode;

    private void Awake()
    {
        if (isMainMenu)
        {
            Application.targetFrameRate = 60;
        }
    }

    private void Start()
    {
        _pirateMode = PlayerPrefs.GetInt("PirateMode", 0) == 1;
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", -10);
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0);
        _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);
        
        audioMixer.SetFloat("MasterVolume", _masterVolume);
        audioMixer.SetFloat("MusicVolume", _musicVolume);
        audioMixer.SetFloat("SfxVolume", _sfxVolume);

        masterVolumeSlider.value = Mathf.Pow(10, _masterVolume / 20);
        musicVolumeSlider.value = Mathf.Pow(10, _musicVolume / 20);
        sfxVolumeSlider.value = Mathf.Pow(10, _sfxVolume / 20);
        pirateModeToggle.isOn = _pirateMode;
        pirateModeText.SetActive(_pirateMode);
        
        if (!isMainMenu)
        {
            if (_pirateMode)
            {
                musicAudioSource.clip = pirateAudioClip;
                musicAudioSource.Play();
            }
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", _sfxVolume);
        PlayerPrefs.SetInt("PirateMode", _pirateMode ? 1 : 0);
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        if (sceneIndex == 0 && GameManager.Instance.inMultiplayerMode)
        {
            NetworkManager.Singleton.Shutdown();
        } 
    }
    

    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void SetMasterVolume(float volume)
    {
        if (volume == 0)
        {
            _masterVolume = -80;
        }
        else
        {
            _masterVolume = Mathf.Log10(volume) * 20;
        }
        
        audioMixer.SetFloat("MasterVolume", _masterVolume);
    }
    
    public void SetMusicVolume(float volume)
    {
        if (volume == 0)
        {
            _musicVolume = -80;
        }
        else
        {
            _musicVolume = Mathf.Log10(volume) * 20;
        }
        
        audioMixer.SetFloat("MusicVolume", _musicVolume);
    }
    
    public void SetSfxVolume(float volume)
    {
        if (volume == 0)
        {
            _sfxVolume = -80;
        }
        else
        {
            _sfxVolume = Mathf.Log10(volume) * 20;
        }
        
        audioMixer.SetFloat("SfxVolume", _sfxVolume);
    }
    
    public void SetPirateMode(bool pirateMode)
    {
        _pirateMode = pirateMode;
        pirateModeText.SetActive(pirateMode);
    }

    private void Update()
    {
        if (!manageScale) return;
        var aspectRatio = (float) Screen.width / Screen.height;
        if (aspectRatio < 16f/9f)
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
    }
}
