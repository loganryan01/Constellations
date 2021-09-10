using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region Fields
    [Header("UI Screens")]
    public GameObject optionsCanvas;
    public GameObject mainMenuCanvas;
    
    [Header("Audio controls")]
    public AudioMixer masterMixer;
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
        }
        else if (Screen.width >= 1920 && Screen.width < 2560)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (Screen.width >= 2560 && Screen.width < 3840)
        {
            Screen.SetResolution(2560, 1440, true);
        }
        else if (Screen.width >= 3840)
        {
            Screen.SetResolution(3840, 2160, true);
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
    public void ChangeQuality(Dropdown dropdown)
    {
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
    public void ChangeScreenResolution(Dropdown dropdown)
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
