using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingController : MonoBehaviour
{
    #region Fields
    [Header("Target Colours")]
    public Color targetColour1 = new Color(0, 0, 0, 0);
    public Color targetColour2 = new Color(0, 0, 0, 0);

    [Header("Target text")]
    public TextMeshProUGUI textToFade;

    [Header("Fade Timer")]
    public float fadeDuration = 3;

    private bool coroutineFinished = false;
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpTextColour(targetColour1, fadeDuration));
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineFinished)
        {
            StartCoroutine(LerpTextColour(targetColour2, fadeDuration));
        }
    }

    IEnumerator LerpTextColour(Color endColour, float duration)
    {
        coroutineFinished = false;

        float time = 0;
        Color startColour = textToFade.color;

        while (time < duration)
        {
            textToFade.color = Color.Lerp(startColour, endColour, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        textToFade.color = endColour;

        coroutineFinished = true;

        if (endColour == targetColour2)
        {
            Application.Quit();
        }
    }
    #endregion
}
