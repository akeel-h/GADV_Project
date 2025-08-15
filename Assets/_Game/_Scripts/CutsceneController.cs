using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public TMP_Text dialogueText;
    public string[] messages;

    public float typeSpeed = 0.05f;
    private int currentMessage = 0;
    private bool typing = false;

    public Image fadeImage; 
    public float fadeDuration = 1f;

    public bool loadMainMenu;

    public AudioSource typingAudio; // assign your typing sound here in the inspector

    private void Start()
    {
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        while (currentMessage < messages.Length)
        {
            typing = true;
            dialogueText.text = "";

            string message = messages[currentMessage];

            // Last message in red
            dialogueText.color = (currentMessage == messages.Length - 1) ? Color.red : Color.white;

            // Typewriter effect
            for (int i = 0; i < message.Length; i++)
            {
                dialogueText.text += message[i];

                // Start playing typing audio if it's not already playing
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

            // Stop typing audio once the message is fully displayed
            if (typingAudio.isPlaying)
                typingAudio.Stop();


            typing = false;

            // Wait until the player clicks to continue
            // Skip the first click if it already finished the typing
            bool clicked = false;
            while (!clicked)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clicked = true;
                    // Wait one frame to avoid double-detection of the same click
                    yield return null;
                }
                else
                {
                    yield return null;
                }
            }

            currentMessage++;
        }

        // Fade to black or load next scene after last message
        StartCoroutine(FadeAndLoadNextScene());
    }



    private IEnumerator FadeAndLoadNextScene()
    {
        // Fade from transparent to black
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Make sure fully black
        color.a = 1f;
        fadeImage.color = color;

        // Load either main menu or next scene
        if (loadMainMenu)
        {
            SceneManager.LoadScene(0); // assuming build index 0 = main menu
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


}
