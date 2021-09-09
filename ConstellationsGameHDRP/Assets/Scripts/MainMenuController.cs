using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject optionsCanvas;
    public GameObject mainMenuCanvas;
    
    public AudioMixer masterMixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SettingsCanvas()
    {
        optionsCanvas.SetActive(!optionsCanvas.activeSelf);
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeSelf);
    }

    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

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

    public void EnableFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    //public void ValueReader(int value)
    //{

    //}

    //void OnGUI()
    //{
    //    //This displays a Button on the screen at position (20,30), width 150 and height 50. The button’s text reads the last parameter. Press this for the SceneManager to load the Scene.
    //    if (GUI.Button(new Rect(20, 30, 150, 30), "Other Scene Single"))
    //    {
    //        //The SceneManager loads your new Scene as a single Scene (not overlapping). This is Single mode.
    //        SceneManager.LoadScene("main", LoadSceneMode.Single);
    //    }

    //    //Whereas pressing this Button loads the Additive Scene.
    //    if (GUI.Button(new Rect(20, 60, 150, 30), "Other Scene Additive"))
    //    {
    //        //SceneManager loads your new Scene as an extra Scene (overlapping the other). This is Additive mode.
    //        SceneManager.LoadScene("main", LoadSceneMode.Additive);
    //    }
    //}
}
