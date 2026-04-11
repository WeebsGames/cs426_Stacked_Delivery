using UnityEngine;
using UnityEngine.UI;
using TMPro;

//
// StabilityMeterUI
//
// Displays vehicle item stack stability value as a vertical bar meter.
// Green = stable, Red = unstable.
// Pulses and shows "Unstable" text when stability is critically low.
// Shows permanent "Boxes Fallen!" text when stability reaches zero.
// Updates in real-time based on ItemPhysics stability value.
//
public class StabilityMeterUI : MonoBehaviour
{
    [Header("References")]
    public ItemPhysics itemPhysics;
    public Image fillImage;
    public TMP_Text unstableText;
    public TMP_Text boxesFallenText;
    public TMP_Text boxesStableText;

    [Header("Colors")]
    public Color stableColor = Color.green;
    public Color unstableColor = Color.red;

    [Header("Unstable text Settings")]
    public float criticalThreshold = 0.5f;
    public float pulseSpeed = 10f;
    public AudioSource source;
    public AudioSource music;
    public AudioClip fail;

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
    // Shows and pulses "Unstable" warning text when stability is critically low.
    // Shows permanent Items Fallen text when stability reaches zero.
    // Shows "Boxes Stable" text when stability is above threshold.
    //
    void UpdateWarning()
    {
        if (unstableText == null) return;

        float stability = itemPhysics.GetStability();
        bool fallen = itemPhysics.GetItemsFallen();

        if (fallen)
        {
            source.Stop();
            source.clip = fail;
            source.Play();
            music.Stop();
            unstableText.gameObject.SetActive(false);
            if (boxesStableText != null)
                boxesStableText.gameObject.SetActive(false);
            if (boxesFallenText != null)
                boxesFallenText.gameObject.SetActive(true);
            GetComponent<StabilityMeterUI>().enabled = false;
        }
        else if (stability <= criticalThreshold)
        {
            source.UnPause();
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            unstableText.gameObject.SetActive(true);
            unstableText.alpha = pulse;
            if (boxesStableText != null)
                boxesStableText.gameObject.SetActive(false);
            if (boxesFallenText != null)
                boxesFallenText.gameObject.SetActive(false);
        }
        else
        {
            source.Pause();
            unstableText.gameObject.SetActive(false);
            if (boxesStableText != null)
                boxesStableText.gameObject.SetActive(true);
            if (boxesFallenText != null)
                boxesFallenText.gameObject.SetActive(false);
        }
    }
}
