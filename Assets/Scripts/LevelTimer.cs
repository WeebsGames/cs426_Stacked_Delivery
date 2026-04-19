using UnityEngine;
using TMPro;

// Counts down level time and updates a UI text display.
public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float timeLimit = 90f;
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning = true;

    // Initialize the countdown.
    private void Start()
    {
        timeRemaining = timeLimit;
    }

    // Count down each frame and update the display, if the timer 
    // is still running.
    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            // fail code if the time runs out
        }

        UpdateTimerText();
    }

    // time as M:SS and update the UI text.
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = minutes + ":" + seconds.ToString("00");
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}