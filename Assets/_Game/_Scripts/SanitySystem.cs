using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity = 0f;

    [Header("Passive Sanity Gain Settings")]
    public float passiveSanityRate = 1f;
    public float passiveSanityTickInterval = 1f;

    [Header("Hiding Sanity Gain Settings")]
    public float hidingSanityRate = 5f;
    public float hidingSanityCooldown = 1f;

    [Header("Hiding While Monster Gain Settings")]
    public float monsterSanityPerTick = 2f;
    public float monsterSanityTickInterval = 1.5f;

    [Header("Chase Sanity Gain")]
    public float chaseSanityGainRate = 5f;

    [Header("High Sanity Death Settings")]
    public float highSanityThreshold = 80f;
    public float highSanityDuration = 5f;

    [Header("UI")]
    public Image sanityBar;

    [HideInInspector] public CabinetHide currentHidingCabinet;

    private Coroutine highSanityCoroutine;
    private Coroutine hidingSanityCoroutine;
    private Coroutine monsterSanityCoroutine;
    private Coroutine passiveSanityCoroutine;
    private bool isDead = false;

    public GameObject player; // Assign in Inspector

    void Start()
    {
        InitializeSanitySystem();
    }

    void Update()
    {
        HandleSanityUpdate();
    }

    // Initializes UI and starts passive sanity gain
    private void InitializeSanitySystem()
    {
        UpdateSanityUI();
        StartPassiveSanityGain();
    }

    // Handles all per-frame sanity logic
    private void HandleSanityUpdate()
    {
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

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

    // Updates the sanity bar UI
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
                    bool monsterNearby = currentHidingCabinet.IsMonsterNearby();
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

    private void DieFromStress(bool monsterNearby = false, bool isHiding = false, CabinetHide cabinet = null)
    {
        if (isDead) return;

        isDead = true;

        if (isHiding && cabinet != null && monsterNearby)
        {
            AnurodelaJumpscare jumpscare = cabinet.GetComponent<AnurodelaJumpscare>();
            if (jumpscare != null)
            {
                jumpscare.PlayJumpscare();
                return;
            }
        }

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

    public void OnStartHiding() => StartHidingSanity();
    public void OnStopHiding() => StopHidingSanity();
    public void OnMonsterEncounter(float sanityIncreaseAmount) => IncreaseSanity(sanityIncreaseAmount);
}
