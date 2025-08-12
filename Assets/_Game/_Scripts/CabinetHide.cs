using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CabinetHide : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject player;
    public GameObject hideImageCanvas; // Canvas with first-person sprite
    public Image hideImageDisplay;     // Image to show sprite
    public float transitionDelay = 1f;
    public float imageDisplayTime = 2f;

    [Header("Sprites")]
    public Sprite[] monsterImages;
    public Sprite[] emptyImages;

    [Header("Detection")]
    public Transform monster;
    public float monsterDetectRange = 5f;

    [Header("Fade Settings")]
    public Image fadeImage;       // Fullscreen black UI Image
    public float fadeDuration = 0.5f;

    private bool isNearCabinet = false;
    private bool isHiding = false;
    private bool monsterNearby = false;
    private bool monsterSeen = false;

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
        monsterSeen = false;

        // Fade to black
        yield return StartCoroutine(Fade(1));

        player.SetActive(false);
        hideImageCanvas.SetActive(true);

        monsterNearby = IsMonsterNearby();
        if (monsterNearby)
        {
            StartCoroutine(PlayRandomImagesWithReroll());
        }
        else
        {
            ShowEmptyImage();
        }

        // Fade from black to scene
        yield return StartCoroutine(Fade(0));
    }

    IEnumerator ExitCabinetSequence()
    {
        // Fade to black
        yield return StartCoroutine(Fade(1));

        hideImageCanvas.SetActive(false);
        yield return new WaitForSeconds(transitionDelay);

        player.SetActive(true);
        isHiding = false;

        // Fade from black
        yield return StartCoroutine(Fade(0));
    }

    void ShowEmptyImage()
    {
        hideImageDisplay.sprite = emptyImages[Random.Range(0, emptyImages.Length)];
    }

    void ShowMonsterImage()
    {
        hideImageDisplay.sprite = monsterImages[Random.Range(0, monsterImages.Length)];
        monsterSeen = true;
    }

    IEnumerator PlayRandomImagesWithReroll()
    {
        while (!monsterSeen)
        {
            bool pickMonster = Random.value > 0.5f;
            if (pickMonster)
                ShowMonsterImage();
            else
                ShowEmptyImage();

            yield return new WaitForSeconds(imageDisplayTime);
        }
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
