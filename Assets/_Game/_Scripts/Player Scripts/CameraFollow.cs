using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;          // The player's transform
    public float smoothSpeed = 0.125f; // How smoothly the camera follows
    public Vector3 offset;            // Offset from the target

    private void LateUpdate()
    {
        FollowTarget();
    }

    // ---------------- Camera Follow ----------------

    // Moves the camera to follow the target
    private void FollowTarget()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = desiredPosition;
    }
}
