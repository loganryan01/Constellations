/*----------------------------------------
    Name: MainMenuController
    Purpose: Controls the main menu scene.
    Author: Logan Ryan and Mara Dusevic
    Modified: 18 November 2021
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
    public Slider audioValue; // Slider that controls the audio of the game
    public TextMeshProUGUI audioValueText; // The text that displays volume of the game

    [Header("Screen Resolution Dropdown")]
    public TMP_Dropdown screenResolutionDropdown; // Dropdown that displays the screen resolutions for the game

    [Header("Mouse Sensitivity Settings")]
    public Slider mouseSensitivityValue; // Slider that controls the sensitivity of the mouse
    public TextMeshProUGUI mouseSensitivityValueText; // Text that displays the value of the sensitivity of the mouse

    [Header("Quality Settings")]
    public TMP_Dropdown qualityDropdown; // Dropdown that displays the qualities for the game

    [Header("Fullscreen Settings")]
    public Toggle fullscreenToggle; // Toggle that activates fullscreen mode
    #endregion

    #region Functions
    // Start function
    void Start()
    {
        // Check if the player has a chosen audio setting
        if (PlayerPrefs.HasKey("Audio"))
        {
            // If it does set it to the chosen setting
            audioValue.value = PlayerPrefs.GetFloat("Audio");
        }
        else
        {
            // Otherwise, set it to the default value of 20
            audioValue.value = 20;
        }

        // Convert the volume to a number between 0 - 100, then to a text
        float audioVolume = 5 / 4 * audioValue.value + 80;
        audioValueText.text = audioVolume.ToString();

        // Set the volume of the game
        masterMixer.SetFloat("musicVol", audioValue.value);

        // Save the audio
        PlayerPrefs.SetFloat("Audio", audioValue.value);

        // Quality
        if (PlayerPrefs.HasKey("Quality"))
        {
            qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            qualityDropdown.value = 0;
        }

        QualitySettings.SetQualityLevel(qualityDropdown.value);

        PlayerPrefs.SetInt("Quality", qualityDropdown.value);

        // Enable the fullscreen
        int fullscreenInt;
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            fullscreenInt = PlayerPrefs.GetInt("Fullscreen");
        }
        else
        {
            fullscreenInt = 1;
        }

        switch (fullscreenInt)
        {
            case 0:
                Screen.fullScreen = false;
                fullscreenToggle.isOn = false;
                break;
            case 1:
                Screen.fullScreen = true;
                fullscreenToggle.isOn = true;
                break;
        }

        // Change the resolution based on the users screen
        if (PlayerPrefs.HasKey("Screen Resolution"))
        {
            screenResolutionDropdown.value = PlayerPrefs.GetInt("Screen Resolution");

            switch (screenResolutionDropdown.value)
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
                    Screen.SetResolution(1920, 1080, Screen.fullScreen);
                    break;
                case 6:
                    Screen.SetResolution(2560, 1440, Screen.fullScreen);
                    break;
                case 7:
                    Screen.SetResolution(3840, 2160, Screen.fullScreen);
                    break;
            }
        }
        else
        {
            if (Screen.width >= 1280 && Screen.width < 1920)
            {
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                screenResolutionDropdown.value = 4;
            }
            else if (Screen.width >= 1920 && Screen.width < 2560)
            {
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                screenResolutionDropdown.value = 5;
            }
            else if (Screen.width >= 2560 && Screen.width < 3840)
            {
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                screenResolutionDropdown.value = 6;
            }
            else if (Screen.width >= 3840)
            {
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                screenResolutionDropdown.value = 7;
            }
        }

        PlayerPrefs.SetInt("Screen Resolution", screenResolutionDropdown.value);

        // Mouse Sensitivity
        if (PlayerPrefs.HasKey("Look Sensitivity"))
        {
            mouseSensitivityValue.value = PlayerPrefs.GetFloat("Look Sensitivity");
        }
        else
        {
            mouseSensitivityValue.value = 15;
        }

        mouseSensitivityValueText.text = mouseSensitivityValue.value.ToString();

        PlayerPrefs.SetFloat("Look Sensitivity", mouseSensitivityValue.value);
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

    public void SetLookSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Look Sensitivity", sensitivity);

        mouseSensitivityValueText.text = sensitivity.ToString();
    }

    // Change the volume of the sound of the game
    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);

        float audioVolume = 5 / 4 * soundLevel + 80;
        audioValueText.text = audioVolume.ToString();

        PlayerPrefs.SetFloat("Audio", soundLevel);
    }

    // Change the quality of the game
    public void ChangeQuality(TMP_Dropdown dropdown)
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

        PlayerPrefs.SetInt("Quality", dropdown.value);
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

        PlayerPrefs.SetInt("Screen Resolution", screenResolutionDropdown.value);
    }

    // Change from fullscreen to window and vice versa
    public void EnableFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;

        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }
    
    // Close the application
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
