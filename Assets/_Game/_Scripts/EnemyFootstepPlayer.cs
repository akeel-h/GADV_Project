using UnityEngine;

public class EnemyFootstepPlayer : MonoBehaviour
{
    public AudioSource audioSource;   // Assign enemy's footstep audio source here
    public AnurodelaPatrol enemy;     // Reference to the enemy script
    public float patrolPitch = 1f;
    public float chasePitch = 1.5f;

    public void PlayFootstep()
    {
        if (enemy != null && enemy.IsMoving())
        {
            if (enemy.IsChasing())
                audioSource.pitch = chasePitch;
            else
                audioSource.pitch = patrolPitch;

            audioSource.Play();
        }
    }

}
