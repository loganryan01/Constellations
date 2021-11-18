/*-----------------------------------------------
    Name: PauseController
    Purpose: Controls the pause menu of the game.
    Author: Logan Ryan and Mara Dusevic
    Modified: 18 November 2021
-------------------------------------------------
    Copyright 2021 Bookshelf Studios
-----------------------------------------------*/

using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    #region Fields
    public GameObject pauseCanvas; // Pause screen
    public CustomPassVolume blurEffect; // The blur effect for the camera
    public PlayerController playerController; // The script for the player

    public static bool gameIsPaused = false; // Is the game paused
    public static bool disablePauseFunctionality = false; // Should the pause functionality be used

    [Header("Audio controls")]
    public AudioMixer masterMixer; // Mixer that controls the audio for the game
    public Slider audioValue; // Slider that controls the audio of the game
    public TextMeshProUGUI audioValueText; // The text that displays volume of the game

    [Header("Quality Settings")]
    public TMP_Dropdown qualityDropdown; // Dropdown that displays the qualities for the game

    [Header("Screen Resolution Settings")]
    public TMP_Dropdown screenResolutionDropdown; // Dropdown that displays the screen resolutions for the game

    [Header("Fullscreen Settings")]
    public Toggle fullscreenToggle; // Toggle that activates fullscreen mode

    [Header("Mouse Sensitivity Settings")]
    public Slider mouseSensitivityValue; // Slider that controls the sensitivity of the mouse
    public TextMeshProUGUI mouseSensitivityValueText; // Text that displays the value of the sensitivity of the mouse

    [SerializeField] private GameObject PauseMenu; // Object containing all pause menu UI
    [SerializeField] private GameObject OptionsMenu; // Object containing all options menu UI
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        // Set time scale to 1
        Time.timeScale = 1;
        
        // Hide the pause menu
        pauseCanvas.SetActive(false);
        gameIsPaused = false;
        blurEffect.customPasses[0].enabled = false;

        // Reading options from the main menu
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

        // Check if the player has a chosen quality setting
        if (PlayerPrefs.HasKey("Quality"))
        {
            // If it does set it to the chosen setting
            qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            // Otherwise, set it to high
            qualityDropdown.value = 0;
        }

        // Set the qulity of the game
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        // Save the quality
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);

        // Check if the player already has a saved screen resolution setting
        if (PlayerPrefs.HasKey("Screen Resolution"))
        {
            // If they do then, switch the screen resolution to the chosen setting
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
            // If not then change the resolution based on the users screen
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

        // Save screen resolution
        PlayerPrefs.SetInt("Screen Resolution", screenResolutionDropdown.value);

        int fullscreenInt;

        // Check if the player already has a saved fullscreen setting
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            fullscreenInt = PlayerPrefs.GetInt("Fullscreen");
        }
        else
        {
            fullscreenInt = 1;
        }

        // Enable/Disable fullscreen mode
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

        // Look Sensitivity
        mouseSensitivityValueText.text = playerController.lookSensitivity.ToString();
    }

    // Pause the game
    public void Pause()
    {
        if (!disablePauseFunctionality)
        {
            // The game is now paused
            gameIsPaused = !gameIsPaused;

            // If the game is paused
            if (gameIsPaused)
            {
                // Set time scale to 0
                Time.timeScale = 0;

                // Display pause screen
                pauseCanvas.SetActive(true);

                // Unlock the mouse
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Set time scale to 1
                Time.timeScale = 1.0f;

                // Hide pause screen
                pauseCanvas.SetActive(false);

                // Lock the mouse
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    
    // Shows the options menu
    public void Options()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        OptionsMenu.SetActive(!OptionsMenu.activeSelf);
    }
    
    // Change the volume of the sound of the game
    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);

        float audioVolume = 5 / 4 * soundLevel + 80;
        audioValueText.text = audioVolume.ToString();
    }

    // Change the quality of the game
    public void SetQuality(TMP_Dropdown dropdown)
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
    public void SetScreenResolution(TMP_Dropdown dropdown)
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

    // Changes the look sensitivity of the player
    public void SetLookSensitivity(float sensitivity)
    {
        playerController.lookSensitivity = sensitivity;

        mouseSensitivityValueText.text = sensitivity.ToString();
    }

    // Change from fullscreen to window and vice versa
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    // Returns player to Main Menu
    public void QuitToTitleScreen()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // Quit the game
    public void QuitToDesktop()
    {
        Application.Quit();
    }

    // Disables the pause key
    public void DisablePauseFunctionality(bool a_bool)
    {
        disablePauseFunctionality = a_bool;
    }
    #endregion
}
