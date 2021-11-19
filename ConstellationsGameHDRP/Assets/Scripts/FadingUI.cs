using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UIFade
{
    public enum FadeType
    {
        FadeIn,
        FadeOut,
        None
    };

    public enum UIType
    {
        Image,
        TextMeshProUGUI
    };

    public GameObject item;
    public UIType type;
    public FadeType fadeType;
    public bool onStart;
    public float duration = 5.0f;
    public float startAlpha;
    public float timeBefore = 0;
    public float timeAfter = 0;
}

public class FadingUI : MonoBehaviour
{
    [Header("UI Element To Fade")]
    [SerializeField] private List<UIFade> elements;

    private void Start()
    {
        foreach (UIFade element in elements)
        {
            if (element.type.ToString() == "Image")
            {
                Image image = element.item.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, element.startAlpha);

                StartCoroutine(Fade(IsFadeOut(element), element));
            }
            else if (element.type.ToString() == "TextMeshProUGUI")
            {
                TextMeshProUGUI text = element.item.GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, element.startAlpha);

                StartCoroutine(Fade(IsFadeOut(element), element));
            }
        }
    }

    private bool IsFadeOut(UIFade element)
    {
        if (element.fadeType.ToString() == "FadeOut")
        {
            return true;
        }

        return false;
    }

    public IEnumerator Fade(bool fadeOut, UIFade element)
    {
        yield return new WaitForSeconds(element.timeBefore);

        if (element.type.ToString() == "Image")
        {
            Image image = element.item.GetComponent<Image>();
            Color color = image.color;

            StartCoroutine(FadeImage(IsFadeOut(element), image, color, element.duration));
        }
        else if (element.type.ToString() == "TextMeshProUGUI")
        {
            TextMeshProUGUI text = element.item.GetComponent<TextMeshProUGUI>();
            Color color = text.color;

            StartCoroutine(FadeText(IsFadeOut(element), text, color, element.duration));
        }
        else
        {
            yield return null;
        }

        yield return new WaitForSeconds(element.timeAfter);
    }

    private IEnumerator FadeText(bool fadeOut, TextMeshProUGUI text, Color color, float duration)
    {
        float time = 0;
        float fade = fadeOut ? 1.0f : 0;
        Color endColor = new Color(text.color.r, text.color.r, text.color.r, fade);

        while (time < duration)
        {
            text.color = Color.Lerp(color, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        text.color = endColor;
    }

    private IEnumerator FadeImage(bool fadeOut, Image image, Color color, float duration)
    {
        float time = 0;
        float fade = fadeOut ? 1.0f : 0;
        Color endColor = new Color(image.color.r, image.color.r, image.color.r, fade);

        while (time < duration)
        {
            image.color = Color.Lerp(color, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = endColor;
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
