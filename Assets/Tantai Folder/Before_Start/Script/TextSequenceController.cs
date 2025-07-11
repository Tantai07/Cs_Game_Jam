using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TextSequenceController : MonoBehaviour
{
    [Header("Main Text Sequence")]
    public List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

    [Header("Final Prompt")]
    public TextMeshProUGUI finalPromptText;

    [Header("Title Text")]
    public TextMeshProUGUI titleText;

    public float fadeSpeed = 2f;

    [Header("Blinking Settings")]
    public float blinkMinAlpha = 0.3f;
    public float blinkMaxAlpha = 1f;
    public float blinkSpeed = 2f;

    private int currentIndex = 0;
    private bool isFading = false;
    private bool isFinalPromptShown = false;
    private bool isReadyToStart = false;
    private bool startBlinking = false;

    private void Start()
    {
        foreach (var text in textList)
            SetAlpha(text, 0f);

        if (finalPromptText != null)
            SetAlpha(finalPromptText, 0f);

        if (titleText != null)
            SetAlpha(titleText, 0f);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isFading)
        {
            if (!isFinalPromptShown)
            {
                if (currentIndex < textList.Count)
                {
                    StartCoroutine(FadeIn(textList[currentIndex]));
                    currentIndex++;
                }
                else
                {
                    StartCoroutine(FadeAllOutAndShowPrompt());
                }
            }
            else if (isReadyToStart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (startBlinking)
            UpdateFinalPromptBlink();
    }

    private void UpdateFinalPromptBlink()
    {
        if (finalPromptText == null) return;

        float alpha = Mathf.Lerp(blinkMinAlpha, blinkMaxAlpha, Mathf.PingPong(Time.time * blinkSpeed, 1f));
        SetAlpha(finalPromptText, alpha);
    }

    private IEnumerator FadeIn(TextMeshProUGUI text)
    {
        isFading = true;
        float alpha = text.color.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetAlpha(text, Mathf.Clamp01(alpha));
            yield return null;
        }

        isFading = false;
    }

    private IEnumerator FadeAllOutAndShowPrompt()
    {
        isFading = true;
        float alpha = 1f;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            foreach (var text in textList)
                SetAlpha(text, Mathf.Clamp01(alpha));
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        if (titleText != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * fadeSpeed;
                SetAlpha(titleText, Mathf.Clamp01(t));
                yield return null;
            }
        }

        if (finalPromptText != null)
        {
            float a = 0f;
            while (a < 1f)
            {
                a += Time.deltaTime * fadeSpeed;
                SetAlpha(finalPromptText, Mathf.Clamp01(a));
                yield return null;
            }

            startBlinking = true;
        }

        isFading = false;
        isFinalPromptShown = true;
        isReadyToStart = true;
    }

    private void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        Color c = text.color;
        text.color = new Color(c.r, c.g, c.b, alpha);
    }
}
