using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CabinetHide : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject player;
    public GameObject hideImageCanvas; // Canvas for hiding view

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

    private bool isNearCabinet = false;
    private bool isHiding = false;
    private Coroutine monsterCoroutine;
    private Coroutine monsterSanityCoroutine;

    private enum CabinetState { Safe, Monster, TransitionToSafe }
    private CabinetState currentState = CabinetState.Safe;

    void Update()
    {
        if (isNearCabinet && !isHiding && Input.GetKeyDown(interactKey))
            StartCoroutine(EnterCabinetSequence());
        else if (isHiding && Input.GetKeyDown(interactKey))
            StartCoroutine(ExitCabinetSequence());
    }

    private IEnumerator MonsterSanityRoutine(float sanityPerTick, float tickInterval)
    {
        while (true)
        {
            sanitySystem?.OnMonsterEncounter(sanityPerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }


    IEnumerator EnterCabinetSequence()
    {
        isHiding = true;
        PlayerHideState.IsHiding = true;  // <-- set hiding state
        sanitySystem?.OnStartHiding();

        // Fade to black
        yield return StartCoroutine(Fade(1));

        player.SetActive(false);
        hideImageCanvas.SetActive(true);

        yield return StartCoroutine(Fade(0));

        currentState = CabinetState.Safe;
        safeImageDisplay.gameObject.SetActive(true);
        monsterImageDisplay.gameObject.SetActive(false);

        while (isHiding)
        {
            bool monsterNearby = IsMonsterNearby();

            if (monsterNearby)
            {
                if (currentState != CabinetState.Monster)
                {
                    currentState = CabinetState.Monster;

                    safeImageDisplay.gameObject.SetActive(false);
                    monsterImageDisplay.gameObject.SetActive(true);

                    if (monsterCoroutine != null)
                        StopCoroutine(monsterCoroutine);

                    monsterCoroutine = StartCoroutine(PlayMonsterAnimation());

                    if (monsterSanityCoroutine == null)
                    {
                        sanitySystem?.StartMonsterSanityGain();
                    }

                }
            }
            else
            {
                if (currentState == CabinetState.Monster)
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
            }

            yield return null;
        }
    }

    IEnumerator ExitCabinetSequence()
    {
        yield return StartCoroutine(Fade(1));

        hideImageCanvas.SetActive(false);
        player.SetActive(true);
        isHiding = false;
        PlayerHideState.IsHiding = false;  // <-- unset hiding state
        sanitySystem?.OnStopHiding();

        yield return StartCoroutine(Fade(0));
    }


    IEnumerator PlayMonsterAnimation()
    {
        for (int frameIndex = 0; frameIndex < monsterFrames.Length; frameIndex++)
        {
            monsterImageDisplay.sprite = monsterFrames[frameIndex];
            yield return new WaitForSeconds(monsterFrameTime);
        }

        // Hold the last frame
        monsterImageDisplay.sprite = monsterFrames[monsterFrames.Length - 1];
        monsterCoroutine = null;
    }


    IEnumerator PlayTransitionAnimation()
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
        monsterCoroutine = null;  // <-- make sure this is set
    }

    bool IsMonsterNearby()
    {
        if (monster == null) return false;
        float dist = Vector2.Distance(player.transform.position, monster.position);
        return dist <= monsterDetectRange;
    }

    IEnumerator Fade(float targetAlpha)
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
            isNearCabinet = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
            isNearCabinet = false;
    }
}
