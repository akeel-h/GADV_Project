using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelFade : MonoBehaviour
{
    public Image fadeImage; // Assign your fullscreen black image
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color c = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }

    public IEnumerator FadeOutCoroutine()
    {
        float timer = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
        fadeImage.gameObject.SetActive(true);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

}
