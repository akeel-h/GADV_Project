using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    [SerializeField] private DoorTileController doorController; // optional
    [SerializeField] private Direction direction;
    [SerializeField] Transform teleportTargetPosition;

    private CinemachineConfiner confiner;

    private enum Direction { Up, Down, Left, Right, Teleport };

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        // If a door exists and it is not open, block transition
        if (doorController != null && !doorController.IsDoorOpen())
            return;

        confiner.m_BoundingShape2D = mapBoundary;
        UpdatePlayerPosition(collision.gameObject);
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        if(direction == Direction.Teleport)
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
}
