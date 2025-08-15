using UnityEngine;

public class AudioListenerHolder : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player != null)
            transform.position = player.position;
    }
}
