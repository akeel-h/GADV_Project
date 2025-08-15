using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    [Header("References")]
    public SanitySystem sanitySystem;   // Reference to your sanity system
    public Image fillImage;             // The fill image of the bar

    [Header("Color Settings")]
    public Color lowSanityColor = Color.green;
    public Color highSanityColor = Color.red;

    [Header("Flashing Settings")]
    public float flashThreshold = 0.8f;  // Start flashing when sanity >= 80%
    public float flashSpeed = 2f;        // How fast the bar flashes

    void Update()
    {
        UpdateSanityBar();
    }

    // Updates the fill amount, color, and flashing of the sanity bar
    private void UpdateSanityBar()
    {
        if (sanitySystem == null || fillImage == null)
            return;

        // Calculate current sanity percentage
        float sanityPercent = sanitySystem.currentSanity / sanitySystem.maxSanity;

        // Update fill amount based on sanity
        fillImage.fillAmount = sanityPercent;

        // Lerp color between low and high sanity
        fillImage.color = Color.Lerp(lowSanityColor, highSanityColor, sanityPercent);

        // Handle flashing when sanity is above threshold
        if (sanityPercent >= flashThreshold)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed)); // oscillates between 0 and 1
            Color flashColor = fillImage.color;
            flashColor.a = 0.5f + 0.5f * alpha; // flicker between 0.5 and 1
            fillImage.color = flashColor;
        }
        else
        {
            // Reset alpha to fully opaque if below threshold
            Color c = fillImage.color;
            c.a = 1f;
            fillImage.color = c;
        }
    }
}
