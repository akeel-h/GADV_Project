using UnityEngine;

public class AudioListenerHolder : MonoBehaviour
{
    public Transform player; // Reference to the player transform

    private void LateUpdate()
    {
        FollowPlayer();
    }

    // ---------------- Audio Listener Follow ----------------

    // Move the audio listener to match the player's position
    private void FollowPlayer()
    {
        if (player == null) return;
        transform.position = player.position;
    }
}
