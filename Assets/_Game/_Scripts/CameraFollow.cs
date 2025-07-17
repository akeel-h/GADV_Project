using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The player's transform
    public float smoothSpeed = 0.125f; // How smoothly the camera catches up
    public Vector3 offset;         // Offset from the target

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
