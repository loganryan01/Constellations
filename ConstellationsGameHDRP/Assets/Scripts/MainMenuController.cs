/*----------------------------------------
    Name: MainMenuController
    Purpose: Controls the main menu scene.
    Authour: Logan Ryan
    Modified: 7 October 2021
------------------------------------------
    Copyright 2021 Bookshelf Studios
----------------------------------------*/
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    #region Fields
    [Header("UI Screens")]
    public GameObject optionsCanvas; // Screen that displays the options for the game
    public GameObject mainMenuCanvas; // Screen that displays the start, options and quit button
    
    [Header("Audio controls")]
    public AudioMixer masterMixer; // Mixer that controls the audio for the game

    [Header("Screen Resolution Dropdown")]
    public Dropdown screenResolutionDropdown; // Dropdown that displays the screen resolutions for the game
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        // Enable the fullscreen
        Screen.fullScreen = true;

        // Change the resolution based on the users screen
        if (Screen.width >= 1280 && Screen.width < 1920)
        {
            Screen.SetResolution(1280, 720, true);
            screenResolutionDropdown.value = 4;
        }
        else if (Screen.width >= 1920 && Screen.width < 2560)
        {
            Screen.SetResolution(1920, 1080, true);
            screenResolutionDropdown.value = 5;
        }
        else if (Screen.width >= 2560 && Screen.width < 3840)
        {
            Screen.SetResolution(2560, 1440, true);
            screenResolutionDropdown.value = 6;
        }
        else if (Screen.width >= 3840)
        {
            Screen.SetResolution(3840, 2160, true);
            screenResolutionDropdown.value = 7;
        }
    }

    // Load the next scene in the build order
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Go to the options screen
    public void SettingsCanvas()
    {
        optionsCanvas.SetActive(!optionsCanvas.activeSelf);
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeSelf);
    }

    // Change the volume of the sound of the game
    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);
    }

    // Close the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Change the quality of the game
    public void ChangeQuality(TMP_Dropdown dropdown)
    {
        Debug.Log(dropdown.options[dropdown.value].text);
        
        switch (dropdown.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
        }
    }

    // Change the screen resolution
    public void ChangeScreenResolution(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                Screen.SetResolution(256, 144, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(426, 240, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(640, 360, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(854, 480, Screen.fullScreen);
                break;
            case 4:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 5:
                Screen.SetResolution(1920, 1080, Screen.fullScreen); // FHD
                break;
            case 6:
                Screen.SetResolution(2560, 1440, Screen.fullScreen); // QHD
                break;
            case 7:
                Screen.SetResolution(3840, 2160, Screen.fullScreen); // 4K
                break;
        }
    }

    // Change from fullscreen to window and vice versa
    public void EnableFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    #endregion
}
