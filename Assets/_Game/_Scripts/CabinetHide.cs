using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CabinetHide : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject player;
    public GameObject hideImageCanvas; // Canvas or panel for the cabinet view

    [Header("Safe/Empty View")]
    public Image safeImageDisplay;
    public Sprite[] emptyImages;

    [Header("Monster View (Sprite Animation)")]
    public Image monsterImageDisplay;
    public Sprite[] monsterFrames;
    public float monsterFrameTime = 0.1f; // Time per frame

    [Header("Transition")]
    public Sprite[] transitionFrames;
    public float transitionFrameTime = 0.1f;


    [Header("Detection")]
    public Transform monster;
    public float monsterDetectRange = 5f;

    [Header("Fade Settings")]
    public Image fadeImage;       // Fullscreen black UI Image
    public float fadeDuration = 0.5f;

    private bool isNearCabinet = false;
    private bool isHiding = false;

    private Coroutine monsterCoroutine;

    private enum CabinetState { Safe, Monster, TransitionToSafe }
    private CabinetState currentState = CabinetState.Safe;

    void Update()
    {
        if (isNearCabinet && !isHiding && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(EnterCabinetSequence());
        }
        else if (isHiding && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(ExitCabinetSequence());
        }
    }

    IEnumerator EnterCabinetSequence()
    {
        isHiding = true;
        PlayerHideState.IsHiding = true;

        // Fade to black
        yield return StartCoroutine(Fade(1));

        // Hide player, show cabinet UI
        player.SetActive(false);
        hideImageCanvas.SetActive(true);

        // Fade back in
        yield return StartCoroutine(Fade(0));

        // Start in safe state
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
                    // Monster just appeared
                    currentState = CabinetState.Monster;

                    safeImageDisplay.gameObject.SetActive(false);
                    monsterImageDisplay.gameObject.SetActive(true);

                    if (monsterCoroutine != null)
                        StopCoroutine(monsterCoroutine);

                    monsterCoroutine = StartCoroutine(PlayMonsterAnimation());
                }
            }
            else
            {
                if (currentState == CabinetState.Monster)
                {
                    // Monster just left start transition
                    currentState = CabinetState.TransitionToSafe;

                    if (monsterCoroutine != null)
                    {
                        StopCoroutine(monsterCoroutine);
                        monsterCoroutine = null;
                    }

                    monsterCoroutine = StartCoroutine(PlayTransitionAnimation());
                }
                else if (currentState == CabinetState.TransitionToSafe)
                {
                    // Do nothing, wait for transition coroutine
                }
                else if (currentState == CabinetState.Safe)
                {
                    // Safe image shows if no monster nearby
                    safeImageDisplay.gameObject.SetActive(true);
                    monsterImageDisplay.gameObject.SetActive(false);
                }
            }

            yield return null;
        }
    }

    IEnumerator PlayMonsterAnimation()
    {
        // Play monster frames once
        for (int i = 0; i < monsterFrames.Length; i++)
        {
            monsterImageDisplay.sprite = monsterFrames[i];
            yield return new WaitForSeconds(monsterFrameTime);
        }

        // Stay on last frame until monster leaves
        monsterImageDisplay.sprite = monsterFrames[monsterFrames.Length - 1];

        monsterCoroutine = null;
    }

    IEnumerator PlayTransitionAnimation()
    {
        // Play transition frames once
        for (int i = 0; i < transitionFrames.Length; i++)
        {
            monsterImageDisplay.sprite = transitionFrames[i];
            monsterImageDisplay.gameObject.SetActive(true);
            safeImageDisplay.gameObject.SetActive(false);
            yield return new WaitForSeconds(transitionFrameTime);
        }

        // Transition finished show safe image
        currentState = CabinetState.Safe;
        monsterImageDisplay.gameObject.SetActive(false);
        safeImageDisplay.gameObject.SetActive(true);

        monsterCoroutine = null;
    }







    IEnumerator ExitCabinetSequence()
    {
        // Fade to black
        yield return StartCoroutine(Fade(1));

        hideImageCanvas.SetActive(false);
        player.SetActive(true);
        isHiding = false;
        PlayerHideState.IsHiding = false;

        // Fade from black
        yield return StartCoroutine(Fade(0));
    }

    void ShowEmptyImage()
    {
        safeImageDisplay.gameObject.SetActive(true);
        monsterImageDisplay.gameObject.SetActive(false);

        safeImageDisplay.sprite = emptyImages[Random.Range(0, emptyImages.Length)];
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
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = t / fadeDuration;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, blend);
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
