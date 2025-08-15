using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;  // Footstep audio source
    public PlayerMovement player;    // Reference to PlayerMovement
    public float basePitch = 1f;     // Normal walking pitch
    public float sprintPitch = 1.5f; // Sprinting pitch

    // ---------------- Footstep ----------------

    // Play footstep sound if player is moving
    public void PlayFootstep()
    {
        if (player == null || audioSource == null) return;

        if (player.IsMoving())
        {
            SetFootstepPitch();
            audioSource.Play();
        }
    }

    // Adjust audio pitch based on movement type
    private void SetFootstepPitch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            audioSource.pitch = sprintPitch;
        else
            audioSource.pitch = basePitch;
    }
}
