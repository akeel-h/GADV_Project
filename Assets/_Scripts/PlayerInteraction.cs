using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public InteractionDetector interactionDetector;
    void Update()
    {
        interactionDetector.OnInteract();
    }
}
