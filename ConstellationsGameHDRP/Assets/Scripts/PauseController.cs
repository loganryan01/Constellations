/*-----------------------------------------------
    Name: PauseController
    Purpose: Controls the pause menu of the game.
    Author: Logan Ryan and Mara Dusevic
    Modified: 11 November 2021
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
    public PlayerController playerController;

    public static bool gameIsPaused = false; // Is the game paused
    public static bool disablePauseFunctionality = false;

    [Header("Audio controls")]
    public AudioMixer masterMixer; // Mixer that controls the audio for the game
    public Slider audioValue;
    public TextMeshProUGUI audioValueText;

    [Header("Quality Settings")]
    public TMP_Dropdown qualityDropdown;

    [Header("Screen Resolution Settings")]
    public TMP_Dropdown screenResolutionDropdown;

    [Header("Fullscreen Settings")]
    public Toggle fullscreenToggle;

    [Header("Mouse Sensitivity Settings")]
    public Slider mouseSensitivityValue;
    public TextMeshProUGUI mouseSensitivityValueText;

    [SerializeField] private GameObject PauseMenu; // Object containing all pause menu UI
    [SerializeField] private GameObject OptionsMenu; // Object containing all options menu UI
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        Time.timeScale = 1;
        
        pauseCanvas.SetActive(false);
        gameIsPaused = false;
        blurEffect.customPasses[0].enabled = false;

        // Reading options from the main menu
        // Audio
        if (PlayerPrefs.HasKey("Audio"))
        {
            audioValue.value = PlayerPrefs.GetFloat("Audio");
        }
        else
        {
            audioValue.value = 80;
        }

        float audioVolume = 5 / 4 * audioValue.value + 80;
        audioValueText.text = audioVolume.ToString();

        // Quality
        if (PlayerPrefs.HasKey("Quality"))
        {
            qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            qualityDropdown.value = 0;
        }

        // Resolution
        if (PlayerPrefs.HasKey("Screen Resolution"))
        {
            screenResolutionDropdown.value = PlayerPrefs.GetInt("Screen Resolution");
        }
        else
        {
            screenResolutionDropdown.value = 5;
        }

        // Fullscreen
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

    public void DisablePauseFunctionality(bool a_bool)
    {
        disablePauseFunctionality = a_bool;
    }
    #endregion
}
