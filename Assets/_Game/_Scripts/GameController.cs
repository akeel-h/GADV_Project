using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    // Singleton instance for global access
    public static GameController Instance;

    [Header("UI")]
    public GameObject deathScreen;       // UI panel shown on player death
    public TMP_Text deathMessageText;    // Text element for death message

    private void Awake()
    {
        InitializeSingleton();
    }

    // Sets up the singleton pattern
    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Triggered when the player dies
    public void PlayerDied(GameObject player, string reason = "default", AudioSource jumpscareAudio = null)
    {
        ShowDeathScreen();
        UpdateDeathMessage(reason);
        PauseGame();
        DisablePlayer(player);
        StopOtherAudio(jumpscareAudio);
        PlayJumpscare(jumpscareAudio);
    }

    private void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    private void UpdateDeathMessage(string reason)
    {
        if (deathMessageText == null) return;

        switch (reason)
        {
            case "stress":
                deathMessageText.text = "stay tough. there aren't any pleasant sights.";
                break;
            case "enemy":
                deathMessageText.text = "watch where you step next time.";
                break;
            default:
                deathMessageText.text = "You died!";
                break;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void DisablePlayer(GameObject player)
    {
        if (player != null)
            player.SetActive(false);
    }

    private void StopOtherAudio(AudioSource exceptionAudio)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSrc in allAudioSources)
        {
            if (audioSrc != exceptionAudio)
                audioSrc.Stop();
        }
    }

    private void PlayJumpscare(AudioSource jumpscareAudio)
    {
        if (jumpscareAudio != null && !jumpscareAudio.isPlaying)
            jumpscareAudio.Play();
    }

    // Reloads the current scene
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarted");
    }

    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Loads the main menu (assumed to be build index 0)
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
