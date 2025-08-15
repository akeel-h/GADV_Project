using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioSource audioSource;  // Assign a footstep audio source here
    public PlayerMovement player;    // Reference to your PlayerMovement script
    public float basePitch = 1f;
    public float sprintPitch = 1.5f;

    public void PlayFootstep()
    {
        if (player != null && player.IsMoving())
        {
            // Adjust pitch based on sprinting
            if (Input.GetKey(KeyCode.LeftShift))
                audioSource.pitch = sprintPitch;
            else
                audioSource.pitch = basePitch;

            audioSource.Play();
        }
    }
}
