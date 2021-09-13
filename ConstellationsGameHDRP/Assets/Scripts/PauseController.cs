using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PauseController : MonoBehaviour
{
    #region Fields
    public GameObject pauseCanvas; // Pause screen
    public CustomPassVolume blurEffect; // The blur effect for the camera

    public static bool gameIsPaused = false; // Is the game paused
    #endregion

    #region Functions
    // Start function
    private void Start()
    {
        pauseCanvas.SetActive(false);
        gameIsPaused = false;
        blurEffect.customPasses[0].enabled = false;
    }

    // Pause the game
    public void Pause()
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

            // Enable blur effect
            blurEffect.customPasses[0].enabled = true;

            // Unlock the mouse
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Set time scale to 1
            Time.timeScale = 1.0f;

            // Hide pause screen
            pauseCanvas.SetActive(false);

            // Disable blur effect
            blurEffect.customPasses[0].enabled = false;

            // Lock the mouse
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
