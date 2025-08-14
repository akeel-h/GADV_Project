using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    public SanitySystem sanitySystem;   // Reference to your sanity system
    public Image fillImage;             // The fill image of the bar
    public Color lowSanityColor = Color.green;
    public Color highSanityColor = Color.red;

    [Header("Flashing Settings")]
    public float flashThreshold = 0.8f;  // Start flashing when sanity >= 80%
    public float flashSpeed = 2f;        // How fast the bar flashes

    void Update()
    {
        if (sanitySystem == null || fillImage == null)
            return;

        // Update fill amount
        fillImage.fillAmount = sanitySystem.currentSanity / sanitySystem.maxSanity;

        // Update bar color
        fillImage.color = Color.Lerp(lowSanityColor, highSanityColor, sanitySystem.currentSanity / sanitySystem.maxSanity);

        // Handle flashing when sanity is high
        if (sanitySystem.currentSanity / sanitySystem.maxSanity >= flashThreshold)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed)); // oscillates 0  1  0
            Color flashColor = fillImage.color;
            flashColor.a = 0.5f + 0.5f * alpha; // flicker between 0.5  1 alpha
            fillImage.color = flashColor;
        }
        else
        {
            Color c = fillImage.color;
            c.a = 1f; // reset alpha
            fillImage.color = c;
        }
    }
}
