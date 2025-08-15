using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CabinetHide : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public GameObject player;
    public GameObject hideImageCanvas;

    [Header("Safe/Empty View")]
    public Image safeImageDisplay;
    public Sprite[] emptyImages;

    [Header("Monster View")]
    public Image monsterImageDisplay;
    public Sprite[] monsterFrames;
    public float monsterFrameTime = 0.1f;

    [Header("Transition")]
    public Sprite[] transitionFrames;
    public float transitionFrameTime = 0.1f;

    [Header("Detection")]
    public Transform monster;
    public float monsterDetectRange = 5f;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    [Header("Sanity System")]
    public SanitySystem sanitySystem;

    [Header("Audio")]
    public AudioSource enterCabinetAudio;
    public AudioSource exitCabinetAudio;
    public AudioSource monsterNearbyAudio;

    private bool isNearCabinet = false;
    public bool isHiding;
    [HideInInspector] public bool monsterNearbyInCabinet;

    private Coroutine monsterCoroutine;
    private Coroutine monsterSanityCoroutine;

    private enum CabinetState { Safe, Monster, TransitionToSafe }
    private CabinetState currentState = CabinetState.Safe;

    private void Update()
    {
        HandleCabinetInteraction();
    }

    // ---------------- Interaction ----------------

    // Handles pressing interact key to enter/exit cabinet
    private void HandleCabinetInteraction()
    {
        if (isNearCabinet && !isHiding && Input.GetKeyDown(interactKey))
            StartCoroutine(EnterCabinetSequence());
        else if (isHiding && Input.GetKeyDown(interactKey))
            StartCoroutine(ExitCabinetSequence());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
            isNearCabinet = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
            isNearCabinet = false;
    }

    // ---------------- Enter / Exit ----------------

    private IEnumerator EnterCabinetSequence()
    {
        isHiding = true;
        PlayerHideState.IsHiding = true;

        sanitySystem?.OnStartHiding();
        if (sanitySystem != null)
            sanitySystem.currentHidingCabinet = this;

        if (enterCabinetAudio != null) enterCabinetAudio.Play();

        yield return StartCoroutine(Fade(1));

        player.SetActive(false);
        hideImageCanvas.SetActive(true);

        yield return StartCoroutine(Fade(0));

        currentState = CabinetState.Safe;
        safeImageDisplay.gameObject.SetActive(true);
        monsterImageDisplay.gameObject.SetActive(false);

        while (isHiding)
        {
            monsterNearbyInCabinet = IsMonsterNearby();
            HandleMonsterState();
            yield return null;
        }
    }

    private IEnumerator ExitCabinetSequence()
    {
        yield return StartCoroutine(Fade(1));

        if (exitCabinetAudio != null) exitCabinetAudio.Play();
        if (monsterNearbyAudio != null && monsterNearbyAudio.isPlaying)
            monsterNearbyAudio.Stop();

        hideImageCanvas.SetActive(false);
        player.SetActive(true);
        isHiding = false;
        PlayerHideState.IsHiding = false;
        sanitySystem?.OnStopHiding();

        yield return StartCoroutine(Fade(0));
    }

    // ---------------- Monster Handling ----------------

    private void HandleMonsterState()
    {
        if (monsterNearbyInCabinet && currentState != CabinetState.Monster)
            StartMonsterEncounter();
        else if (!monsterNearbyInCabinet && currentState == CabinetState.Monster)
            StartTransitionToSafe();
    }

    private void StartMonsterEncounter()
    {
        currentState = CabinetState.Monster;
        safeImageDisplay.gameObject.SetActive(false);
        monsterImageDisplay.gameObject.SetActive(true);

        if (monsterCoroutine != null) StopCoroutine(monsterCoroutine);
        monsterCoroutine = StartCoroutine(PlayMonsterAnimation());

        if (monsterSanityCoroutine == null)
        {
            sanitySystem?.StartMonsterSanityGain();
        }

        if (monsterNearbyAudio != null && !monsterNearbyAudio.isPlaying)
            monsterNearbyAudio.Play();
    }

    private void StartTransitionToSafe()
    {
        currentState = CabinetState.TransitionToSafe;
        sanitySystem?.StopMonsterSanityGain();

        monsterCoroutine = StartCoroutine(PlayTransitionAnimation());

        if (monsterSanityCoroutine != null)
        {
            StopCoroutine(monsterSanityCoroutine);
            monsterSanityCoroutine = null;
        }
    }

    public bool IsMonsterNearby()
    {
        if (monster == null) return false;
        float dist = Vector2.Distance(player.transform.position, monster.position);
        return dist <= monsterDetectRange;
    }

    // ---------------- Animations ----------------

    private IEnumerator PlayMonsterAnimation()
    {
        for (int i = 0; i < monsterFrames.Length; i++)
        {
            monsterImageDisplay.sprite = monsterFrames[i];
            yield return new WaitForSeconds(monsterFrameTime);
        }

        monsterImageDisplay.sprite = monsterFrames[monsterFrames.Length - 1];
        monsterCoroutine = null;
    }

    private IEnumerator PlayTransitionAnimation()
    {
        foreach (var frame in transitionFrames)
        {
            monsterImageDisplay.sprite = frame;
            monsterImageDisplay.gameObject.SetActive(true);
            safeImageDisplay.gameObject.SetActive(false);
            yield return new WaitForSeconds(transitionFrameTime);
        }

        currentState = CabinetState.Safe;
        monsterImageDisplay.gameObject.SetActive(false);
        safeImageDisplay.gameObject.SetActive(true);
        monsterCoroutine = null;
    }

    // ---------------- Fade ----------------

    private IEnumerator Fade(float targetAlpha)
    {
        Color c = fadeImage.color;
        float startAlpha = c.a;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        fadeImage.color = c;
    }

    // ---------------- Monster Sanity ----------------

    private IEnumerator MonsterSanityRoutine(float sanityPerTick, float tickInterval)
    {
        while (true)
        {
            sanitySystem?.OnMonsterEncounter(sanityPerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
