using UnityEngine;
using UnityEngine.UI;
using TMPro;

//
// StabilityMeterUI
//
// Displays vehicle item stack stability value as a vertical bar meter.
// Green = stable, Red = unstable.
// Pulses and shows warning text when stability is critically low.
// Shows permanent "Items Fallen!" text when stability reaches zero.
// Updates in real-time based on ItemPhysics stability value.
//
public class StabilityMeterUI : MonoBehaviour
{
    [Header("References")]
    public ItemPhysics itemPhysics;
    public Image fillImage;
    public TMP_Text warningText;
    public TMP_Text itemsFallenText;

    [Header("Colors")]
    public Color stableColor = Color.green;
    public Color unstableColor = Color.red;

    [Header("Warning Settings")]
    public float criticalThreshold = 0.3f;
    public float pulseSpeed = 5f;

    void Update()
    {
        if (itemPhysics == null || fillImage == null) return;
        UpdateMeter();
        UpdateWarning();
    }

    //
    // UpdateMeter()
    //
    // Updates meter fill amount and color based on current stability.
    // Pulses the fill image when stability is critically low.
    // Stability ranges from 0 (unstable) to 1 (stable).
    //
    void UpdateMeter()
    {
        float stability = itemPhysics.GetStability();
        fillImage.fillAmount = stability;

        if (stability <= criticalThreshold)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            fillImage.color = Color.Lerp(unstableColor, Color.white, pulse);
        }
        else
        {
            fillImage.color = Color.Lerp(unstableColor, stableColor, stability);
        }
    }

    //
    // UpdateWarning()
    //
    // Shows and pulses UNSTABLE warning text when stability is critically low.
    // Shows permanent Items Fallen text when stability reaches zero.
    //
    void UpdateWarning()
    {
        if (warningText == null) return;

        float stability = itemPhysics.GetStability();
        bool fallen = itemPhysics.GetItemsFallen();

        if (fallen)
        {
            warningText.gameObject.SetActive(false);
            if (itemsFallenText != null)
                itemsFallenText.gameObject.SetActive(true);
        }
        else if (stability <= criticalThreshold)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            warningText.gameObject.SetActive(true);
            warningText.alpha = pulse;

            if (itemsFallenText != null)
                itemsFallenText.gameObject.SetActive(false);
        }
        else
        {
            warningText.gameObject.SetActive(false);
            if (itemsFallenText != null)
                itemsFallenText.gameObject.SetActive(false);
        }
    }
}
