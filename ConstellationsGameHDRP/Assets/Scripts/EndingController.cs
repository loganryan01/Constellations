using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    #region Fields

    [Header("Target Colours")]
    [SerializeField] private Color targetColour1 = new Color(0, 0, 0, 0);
    [SerializeField] private Color targetColour2 = new Color(0, 0, 0, 0);

    [Header("Fade Text")]
    [SerializeField] private TextMeshProUGUI finishGameText;
    [SerializeField] private List<TextMeshProUGUI> creditsText;

    [Header("Fade Timer")]
    [SerializeField] private float endGameSpeed = 4;
    [SerializeField] private float creditsSpeed = 6;

    #endregion

    #region Functions

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        // Game Completion Text

        StartCoroutine(LerpTextColour(targetColour1, endGameSpeed, finishGameText));

        yield return new WaitForSeconds(endGameSpeed);

        StartCoroutine(LerpTextColour(targetColour2, endGameSpeed, finishGameText));

        yield return new WaitForSeconds(endGameSpeed);

        // Credits Text

        foreach (TextMeshProUGUI text in creditsText)
        {
            StartCoroutine(LerpTextColour(targetColour1, creditsSpeed, text));
        }

        yield return new WaitForSeconds(creditsSpeed);

        foreach (TextMeshProUGUI text in creditsText)
        {
            StartCoroutine(LerpTextColour(targetColour2, creditsSpeed, text));
        }

        yield return new WaitForSeconds(creditsSpeed);

        SceneManager.LoadScene(0);
    }

    private IEnumerator LerpTextColour(Color endColour, float duration, TextMeshProUGUI text)
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
