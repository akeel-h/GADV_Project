using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("UI")]
    public GameObject deathScreen;
    public TMP_Text deathMessageText;

    private void Awake()
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

    public void PlayerDied(GameObject player, string reason = "default")
    {
        // Show the death screen
        deathScreen.SetActive(true);

        // Update the message
        if (deathMessageText != null)
        {
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

        // Disable player
        player.SetActive(false);
    }


    public void RestartScene()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarted");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
