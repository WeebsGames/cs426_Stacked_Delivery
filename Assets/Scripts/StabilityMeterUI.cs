using UnityEngine;
using UnityEngine.UI;

//
// StabilityMeterUI
//
// Very simple UI to display stack stability
// Displays vehicle item stack stability value as a vertical bar meter.
// Green = stable, Red = unstable.
// Updates in real-time based on ItemPhysics stability value.
//
public class StabilityMeterUI : MonoBehaviour
{
    [Header("References")]
    public ItemPhysics itemPhysics;
    public Image fillImage;

    [Header("Colors")]
    public Color stableColor = Color.green;
    public Color unstableColor = Color.red;

    void Update()
    {
        if (itemPhysics == null || fillImage == null) return;

        UpdateMeter();
    }

    //
    // UpdateMeter()
    //
    // Updates meter fill amount and color based on current stability.
    // Stability ranges from 0 (unstable) to 1 (stable).
    //
    void UpdateMeter()
    {
        float stability = itemPhysics.GetStability();
        fillImage.fillAmount = stability;
        fillImage.color = Color.Lerp(unstableColor, stableColor, stability);
    }
}
