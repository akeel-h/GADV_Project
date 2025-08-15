using UnityEngine;

public class FlashlightRotation : MonoBehaviour
{
    private void Update()
    {
        RotateTowardsMouse();
    }

    // ---------------- Rotation ----------------

    // Rotate the flashlight to point toward the mouse position
    private void RotateTowardsMouse()
    {
        if (Camera.main == null) return;

        // Get mouse position in world space
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Calculate direction vector
        Vector3 direction = mouseWorld - transform.position;

        // Calculate angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate flashlight; +90f adjusts for sprite orientation
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }
}
