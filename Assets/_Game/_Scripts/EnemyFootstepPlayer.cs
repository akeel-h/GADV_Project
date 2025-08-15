using UnityEngine;

public class EnemyFootstepPlayer : MonoBehaviour
{
    // Audio source for the enemy's footstep sounds
    public AudioSource audioSource;

    // Reference to the enemy patrol script
    public AnurodelaPatrol enemy;

    // Pitch when the enemy is patrolling
    public float patrolPitch = 1f;

    // Pitch when the enemy is chasing the player
    public float chasePitch = 1.5f;

    // Play a footstep sound if the enemy is moving
    public void PlayFootstep()
    {
        if (enemy != null && enemy.IsMoving())
        {
            // Adjust pitch depending on enemy state
            if (enemy.IsChasing())
                audioSource.pitch = chasePitch;
            else
                audioSource.pitch = patrolPitch;

            // Play the footstep sound
            audioSource.Play();
        }
    }
}
