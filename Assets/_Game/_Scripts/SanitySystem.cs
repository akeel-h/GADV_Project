using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity = 0f;

    [Header("Sanity Gain Settings")]
    public float hidingSanityRate = 5f; // sanity per second while hiding
    public float hidingSanityCooldown = 1f; // check interval

    [Header("High Sanity Death Settings")]
    public float highSanityThreshold = 80f;
    public float highSanityDuration = 5f; // Seconds before death if above threshold

    [Header("UI")]
    public Image sanityBar; // Assign your filled image here

    private Coroutine highSanityCoroutine;
    private Coroutine hidingSanityCoroutine;
    private bool isDead = false;

    void Start()
    {
        UpdateSanityUI();
    }

    void Update()
    {
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        // Instant death at max sanity
        if (currentSanity >= maxSanity && !isDead)
        {
            DieFromStress();
        }
        else if (currentSanity >= highSanityThreshold)
        {
            if (highSanityCoroutine == null)
                highSanityCoroutine = StartCoroutine(HighSanityTimer());
        }
        else
        {
            if (highSanityCoroutine != null)
            {
                StopCoroutine(highSanityCoroutine);
                highSanityCoroutine = null;
            }
        }

        UpdateSanityUI();
    }

    private void UpdateSanityUI()
    {
        if (sanityBar != null)
            sanityBar.fillAmount = currentSanity / maxSanity;
    }

    public void IncreaseSanity(float amount)
    {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
    }

    public void ReduceSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
    }

    public void StartHidingSanity()
    {
        if (hidingSanityCoroutine == null)
            hidingSanityCoroutine = StartCoroutine(HidingSanityRoutine());
    }

    public void StopHidingSanity()
    {
        if (hidingSanityCoroutine != null)
        {
            StopCoroutine(hidingSanityCoroutine);
            hidingSanityCoroutine = null;
        }
    }

    private IEnumerator HidingSanityRoutine()
    {
        while (true)
        {
            IncreaseSanity(hidingSanityRate * hidingSanityCooldown);
            yield return new WaitForSeconds(hidingSanityCooldown);
        }
    }

    private IEnumerator HighSanityTimer()
    {
        float timer = 0f;
        while (timer < highSanityDuration)
        {
            timer += Time.deltaTime;
            if (currentSanity < highSanityThreshold)
            {
                highSanityCoroutine = null;
                yield break;
            }
            yield return null;
        }

        DieFromStress();
    }

    private void DieFromStress()
    {
        if (isDead) return;
        isDead = true;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GameController.Instance.PlayerDied(player, "stress");
            Debug.Log("Player has died due to sanity!");
        }
    }


    // Called when player starts hiding
    public void OnStartHiding()
    {
        StartHidingSanity();
    }

    // Called when player stops hiding
    public void OnStopHiding()
    {
        StopHidingSanity();
    }

    // Called when player encounters monster
    public void OnMonsterEncounter(float sanityIncreaseAmount)
    {
        IncreaseSanity(sanityIncreaseAmount);
    }
}
