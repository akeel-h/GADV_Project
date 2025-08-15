using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text dialogueText;        // Text field to display dialogue
    public Image fadeImage;              // Image for fade effect

    [Header("Dialogue Settings")]
    public string[] messages;            // Array of messages to display
    public float typeSpeed = 0.05f;      // Time between each character typed

    [Header("Scene Settings")]
    public bool loadMainMenu;            // Should load main menu after cutscene
    public float fadeDuration = 1f;      // Duration of fade effect

    [Header("Audio")]
    public AudioSource typingAudio;      // Typing sound effect

    private int currentMessage = 0;      // Index of current message
    private bool typing = false;         // Is currently typing a message

    private void Start()
    {
        // Begin the cutscene sequence
        StartCoroutine(ShowMessages());
    }

    // Coroutine to display messages with typewriter effect
    private IEnumerator ShowMessages()
    {
        while (currentMessage < messages.Length)
        {
            typing = true;
            dialogueText.text = "";

            string message = messages[currentMessage];

            // Last message in red, others in white
            dialogueText.color = (currentMessage == messages.Length - 1) ? Color.red : Color.white;

            // Typewriter effect
            for (int i = 0; i < message.Length; i++)
            {
                dialogueText.text += message[i];

                if (!typingAudio.isPlaying)
                    typingAudio.Play();

                // If player clicks, show full message instantly
                if (Input.GetMouseButtonDown(0))
                {
                    dialogueText.text = message;
                    break;
                }

                yield return new WaitForSeconds(typeSpeed);
            }

            if (typingAudio.isPlaying)
                typingAudio.Stop();

            typing = false;

            // Wait for player click to continue
            bool clicked = false;
            while (!clicked)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clicked = true;
                    yield return null; // Skip one frame to avoid double click
                }
                else
                {
                    yield return null;
                }
            }

            currentMessage++;
        }

        // Fade and load next scene after last message
        StartCoroutine(FadeAndLoadNextScene());
    }

    // Coroutine to fade screen and load next scene
    private IEnumerator FadeAndLoadNextScene()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        // Fade from transparent to black
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Ensure fully black
        color.a = 1f;
        fadeImage.color = color;

        // Load either main menu or next scene
        if (loadMainMenu)
            SceneManager.LoadScene(0); // Build index 0 = main menu
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
