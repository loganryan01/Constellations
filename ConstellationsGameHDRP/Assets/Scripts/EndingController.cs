using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    #region Fields
    [Header("Target Colours")]
    public Color targetColour1 = new Color(0, 0, 0, 0);
    public Color targetColour2 = new Color(0, 0, 0, 0);

    [Header("Target text")]
    public TextMeshProUGUI textToFade;
    public TextMeshProUGUI creditsText;

    [Header("Fade Timer")]
    public float fadeDuration = 3;
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeText());
    }

    // Update is called once per frame
    void Update()
    {
        //if (coroutineFinished)
        //{
        //    StartCoroutine(LerpTextColour(targetColour2, fadeDuration));
        //}
    }

    //void FadeText()
    //{

    //}

    IEnumerator FadeText()
    {
        StartCoroutine(LerpTextColour(targetColour1, fadeDuration, textToFade));

        yield return new WaitForSeconds(fadeDuration);

        StartCoroutine(LerpTextColour(targetColour2, fadeDuration, textToFade));

        yield return new WaitForSeconds(fadeDuration);

        StartCoroutine(LerpTextColour(targetColour1, fadeDuration, creditsText));

        yield return new WaitForSeconds(fadeDuration);

        StartCoroutine(LerpTextColour(targetColour2, fadeDuration, creditsText));

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(0);
    }

    IEnumerator LerpTextColour(Color endColour, float duration, TextMeshProUGUI text)
    {
        float time = 0;
        Color startColour = text.color;

        while (time < duration)
        {
            text.color = Color.Lerp(startColour, endColour, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        text.color = endColour;
    }
    #endregion
}
