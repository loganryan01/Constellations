using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
