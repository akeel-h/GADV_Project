using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private PolygonCollider2D mapBoundary;
    [SerializeField] private DoorTileController doorController; // optional
    [SerializeField] private Direction direction;
    [SerializeField] private Transform teleportTargetPosition;

    [Header("Cutscene Option")]
    [SerializeField] private bool loadCutscene = false;
    [SerializeField] private string cutsceneSceneName; // name of cutscene scene to load

    private CinemachineConfiner confiner;

    private enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        InitializeConfiner();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandlePlayerCollision(collision);
    }

    // Initializes the Cinemachine confiner reference
    private void InitializeConfiner()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    // Handles what happens when the player enters the transition trigger
    private void HandlePlayerCollision(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        // If there is a door and it is not open, block transition
        if (doorController != null && !doorController.IsDoorOpen())
            return;

        // Load cutscene if option enabled
        if (loadCutscene)
            LoadCutscene();

        // Update camera confiner and move player
        UpdateConfiner();
        UpdatePlayerPosition(collision.gameObject);
    }

    // Updates the camera confiner to the new map boundary
    private void UpdateConfiner()
    {
        if (confiner != null)
            confiner.m_BoundingShape2D = mapBoundary;
    }

    // Moves the player according to the transition direction
    private void UpdatePlayerPosition(GameObject player)
    {
        if (direction == Direction.Teleport && teleportTargetPosition != null)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up: newPos.y += 2; break;
            case Direction.Down: newPos.y -= 2; break;
            case Direction.Left: newPos.x -= 2; break;
            case Direction.Right: newPos.x += 2; break;
        }

        player.transform.position = newPos;
    }

    // Loads the cutscene scene
    private void LoadCutscene()
    {
        if (!string.IsNullOrEmpty(cutsceneSceneName))
            SceneManager.LoadScene(cutsceneSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
