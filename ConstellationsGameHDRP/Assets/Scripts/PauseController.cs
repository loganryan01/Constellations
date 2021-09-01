using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PauseController : MonoBehaviour
{
    public GameObject pauseCanvas;
    public CustomPassVolume blurEffect;

    public static bool gameIsPaused = false;

    private void Start()
    {
        pauseCanvas.SetActive(false);
        gameIsPaused = false;
        blurEffect.customPasses[0].enabled = false;
    }

    public void Pause()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            blurEffect.customPasses[0].enabled = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseCanvas.SetActive(false);
            blurEffect.customPasses[0].enabled = false;
        }
    }

    public void Quit()
    {
        Debug.Log("Returning to main menu or Quitting game");
    }
}
