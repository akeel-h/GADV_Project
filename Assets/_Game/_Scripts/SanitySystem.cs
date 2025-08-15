using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity = 0f;

    [Header("Passive Sanity Gain Settings")]
    public float passiveSanityRate = 1f; // sanity per second
    public float passiveSanityTickInterval = 1f; // interval between gains

    [Header("Hiding Sanity Gain Settings")]
    public float hidingSanityRate = 5f; // sanity per second while hiding
    public float hidingSanityCooldown = 1f; // check interval

    [Header("Hiding While Monster Gain Settings")]
    public float monsterSanityPerTick = 2f;        // How much sanity to add per tick
    public float monsterSanityTickInterval = 1.5f; // Time between ticks in seconds

    [Header("Chase Sanity Gain")]
    public float chaseSanityGainRate = 5f; // sanity per second while being chased

    [Header("High Sanity Death Settings")]
    public float highSanityThreshold = 80f;
    public float highSanityDuration = 5f; // Seconds before death if above threshold

    [Header("UI")]
    public Image sanityBar; // Assign your filled image here

    [HideInInspector] public CabinetHide currentHidingCabinet; // the cabinet player is hiding in



    private Coroutine highSanityCoroutine;
    private Coroutine hidingSanityCoroutine;
    private Coroutine monsterSanityCoroutine;
    private Coroutine passiveSanityCoroutine;
    private bool isDead = false;

    void Start()
    {
        UpdateSanityUI();
        StartPassiveSanityGain();
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

    public void StartPassiveSanityGain()
    {
        if (passiveSanityCoroutine == null)
            passiveSanityCoroutine = StartCoroutine(PassiveSanityRoutine());
    }

    public void StopPassiveSanityGain()
    {
        if (passiveSanityCoroutine != null)
        {
            StopCoroutine(passiveSanityCoroutine);
            passiveSanityCoroutine = null;
        }
    }

    private IEnumerator PassiveSanityRoutine()
    {
        while (true)
        {
            IncreaseSanity(passiveSanityRate);
            yield return new WaitForSeconds(passiveSanityTickInterval);
        }
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

            if (currentSanity >= maxSanity && !isDead)
            {
                if (currentHidingCabinet != null)
                {
                    bool monsterNearby = currentHidingCabinet.IsMonsterNearby(); // check if monster is outside but near
                    DieFromStress(monsterNearby: monsterNearby, isHiding: true, cabinet: currentHidingCabinet);
                }
                else
                {
                    DieFromStress();
                }
                yield break;
            }


            yield return new WaitForSeconds(hidingSanityCooldown);
        }
    }


    public void StartMonsterSanityGain()
    {
        if (monsterSanityCoroutine == null)
            monsterSanityCoroutine = StartCoroutine(MonsterSanityRoutine());
    }

    public void StopMonsterSanityGain()
    {
        if (monsterSanityCoroutine != null)
        {
            StopCoroutine(monsterSanityCoroutine);
            monsterSanityCoroutine = null;
        }
    }

    private IEnumerator MonsterSanityRoutine()
    {
        while (true)
        {
            IncreaseSanity(monsterSanityPerTick);

            // Optional: death check inside here if you want immediate reaction
            if (currentSanity >= maxSanity && !isDead)
            {
                CabinetHide cabinet = FindObjectOfType<CabinetHide>();
                bool monsterNearby = cabinet != null && cabinet.monsterNearbyInCabinet;
                DieFromStress(monsterNearby: monsterNearby, isHiding: cabinet != null && cabinet.isHiding);
                yield break;
            }


            yield return new WaitForSeconds(monsterSanityTickInterval);
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

    public GameObject player; // Assign in Inspector

    private void DieFromStress(bool monsterNearby = false, bool isHiding = false, CabinetHide cabinet = null)
    {
        if (isDead) return; // already dead, do nothing

        isDead = true;

        // If hiding and monster is nearby, play jumpscare
        if (isHiding && cabinet != null && monsterNearby)
        {
            AnurodelaJumpscare jumpscare = cabinet.GetComponent<AnurodelaJumpscare>();
            if (jumpscare != null)
            {
                jumpscare.PlayJumpscare();
                return; // stop here, jumpscare handles death after animation
            }
        }

        // fallback death
        if (player != null)
        {
            GameController.Instance.PlayerDied(player, "stress");
            Debug.Log("Player has died due to sanity!");
        }
        else
        {
            Debug.LogError("[SanitySystem] Player reference is missing!");
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
