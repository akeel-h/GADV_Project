using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnurodelaJumpscare : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    public Image jumpscareImage;     // UI image for jumpscare animation
    public Sprite[] frames;          // Animation frames
    public float frameTime = 0.1f;   // Time between frames

    [Header("Audio")]
    public AudioSource jumpscareAudio; // Jumpscare sound

    // Trigger the jumpscare sequence
    public void PlayJumpscare()
    {
        gameObject.SetActive(true);

        // Play sound effect
        if (jumpscareAudio != null)
            jumpscareAudio.Play();

        // Pause game time
        Time.timeScale = 0f;

        // Start jumpscare animation
        StartCoroutine(JumpscareRoutine());
    }

    // Runs the jumpscare animation frame-by-frame
    private IEnumerator JumpscareRoutine()
    {
        jumpscareImage.gameObject.SetActive(true);

        // Cycle through each animation frame
        foreach (var frame in frames)
        {
            jumpscareImage.sprite = frame;
            yield return new WaitForSecondsRealtime(frameTime); // Unaffected by timeScale
        }

        // After animation, trigger death sequence
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && GameController.Instance != null)
        {
            GameController.Instance.PlayerDied(player, "stress", jumpscareAudio);

            // Ensure game stays paused for death screen
            Time.timeScale = 0f;
        }
    }
}
