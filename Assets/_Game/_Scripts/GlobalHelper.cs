using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalHelper
{
    // Generates a unique string ID for a GameObject based on its scene and position
    public static string GenerateUniqueID(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("GenerateUniqueID called with null GameObject");
            return "null_object";
        }

        // Format: SceneName_Xpos_Ypos
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}";
    }
}
