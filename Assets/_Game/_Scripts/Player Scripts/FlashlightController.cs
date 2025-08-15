using UnityEngine;

public class FlashlightRotation : MonoBehaviour
{
    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 direction = mouseWorld - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip the angle so it points correctly
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f); // try +90 instead of -90
    }
}
