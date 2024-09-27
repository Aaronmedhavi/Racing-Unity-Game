using UnityEngine;
using TMPro;

public class UIManagerTMP : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text speedometerTMP;    // Reference to the Speedometer TMP Text
    public TMP_Text gearIndicatorTMP;  // Reference to the Gear Indicator TMP Text
    public TMP_Text timerTMP;          // Reference to the Timer TMP Text

    [Header("Car Controller")]
    public CarController carController; // Reference to the CarController script

    private float startTime;            // Time when the race starts
    private bool raceStarted = false;   // Flag to control when the timer starts

    void Start()
    {
        // Check if CarController is assigned
        if (carController == null)
        {
            Debug.LogError("CarController reference not set in UIManagerTMP.");
            return;
        }

        // Initialize UI elements with default values
        UpdateSpeedometer(0f);
        UpdateGearIndicator(carController.CurrentGear);
        UpdateTimer(0f);

        // Start the timer
        startTime = Time.time;
        raceStarted = true;
    }

    void Update()
    {
        if (raceStarted)
        {
            // Get the current speed from CarController
            float speed = carController.GetCurrentSpeed();
            UpdateSpeedometer(speed);

            // Get the current gear from CarController
            int gear = carController.CurrentGear;
            UpdateGearIndicator(gear);

            // Calculate elapsed time
            float elapsedTime = Time.time - startTime;
            UpdateTimer(elapsedTime);
        }
    }

    /// <summary>
    /// Updates the Speedometer UI with the current speed.
    /// </summary>
    /// <param name="speed">Current speed in km/h.</param>
    private void UpdateSpeedometer(float speed)
    {
        speedometerTMP.text = $"{speed:F1} km/h";
    }

    /// <summary>
    /// Updates the Gear Indicator UI with the current gear.
    /// </summary>
    /// <param name="gear">Current gear as an integer.</param>
    private void UpdateGearIndicator(int gear)
    {
        // Convert gear number to string representation
        string gearString = gear switch
        {
            -1 => "R", // Reverse
            0 => "N",  // Neutral
            _ => gear.ToString() // Forward gears
        };
        gearIndicatorTMP.text = $"{gearString}";
    }

    /// <summary>
    /// Updates the Timer UI with the elapsed time.
    /// </summary>
    /// <param name="elapsedTime">Elapsed time in seconds.</param>
    private void UpdateTimer(float elapsedTime)
    {
        // Convert elapsed time to minutes, seconds, and milliseconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime - minutes * 60 - seconds) * 1000);

        // Format the time string
        timerTMP.text = string.Format("Time: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
