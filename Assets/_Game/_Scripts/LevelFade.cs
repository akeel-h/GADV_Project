using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelFade : MonoBehaviour
{
    public Image fadeImage; // Assign your fullscreen black image
    public float fadeDuration = 1f; // Duration of the fade

    void Start()
    {
        StartCoroutine(FadeIn()); // Start fade-in on scene start
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f; // Track elapsed time
        Color c = fadeImage.color; // Get initial color

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; // Increment timer
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration); // Lerp alpha from 1 to 0
            fadeImage.color = c; // Apply color
            yield return null; // Wait a frame
        }

        c.a = 0f; // Ensure fully transparent
        fadeImage.color = c; // Apply final color
    }

    public IEnumerator FadeOutCoroutine()
    {
        float timer = 0f; // Track elapsed time
        Color c = fadeImage.color; // Get initial color
        c.a = 0f; // Start transparent
        fadeImage.color = c; // Apply initial color
        fadeImage.gameObject.SetActive(true); // Ensure image is active

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; // Increment timer
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration); // Lerp alpha from 0 to 1
            fadeImage.color = c; // Apply color
            yield return null; // Wait a frame
        }

        c.a = 1f; // Ensure fully opaque
        fadeImage.color = c; // Apply final color
    }
}
