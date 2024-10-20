using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Speedometer Settings")]
    public RectTransform needleTransform;
    public TMP_Text speedText;
    public float maxSpeed = 200f;
    public float maxNeedleAngle = -220f;
    public float minNeedleAngle = 40f;

    [Header("Position Indicator")]
    public TMP_Text positionText;

    [Header("Lap Indicator")]
    public TMP_Text lapText;

    [Header("Race Timer")]
    public TMP_Text timerText;

    [Header("References")]
    public Rigidbody playerCarRigidbody;

    private int totalRacers;
    private GameObject[] racers;
    private float raceStartTime;
    private bool raceStarted = false;

    void Start()
    {
        raceStartTime = Time.time;
        raceStarted = true;
        racers = GameObject.FindGameObjectsWithTag("Racer");
        totalRacers = racers.Length;
    }

    void Update()
    {
        UpdateSpeedometer();
        UpdatePositionIndicator();
        UpdateLapIndicator();

        if (raceStarted)
        {
            UpdateRaceTimer();
        }
    }

    void UpdateSpeedometer()
    {
        float speed = playerCarRigidbody.velocity.magnitude * 3.6f;
        speed = Mathf.Clamp(speed, 0f, maxSpeed);
        speedText.text = speed.ToString("000");
        float needleAngle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, speed / maxSpeed);
        needleTransform.localEulerAngles = new Vector3(0, 0, needleAngle);
    }

    void UpdatePositionIndicator()
    {
        int currentPosition = CalculatePlayerPosition();
        positionText.text = currentPosition.ToString() + " / " + totalRacers.ToString();
    }

    int CalculatePlayerPosition()
    {
        int position = 1;

        foreach (GameObject racer in racers)
        {
            if (racer != playerCarRigidbody.gameObject)
            {
                float racerProgress = GetCarProgress(racer);
                float playerProgress = GetCarProgress(playerCarRigidbody.gameObject);

                if (racerProgress > playerProgress)
                {
                    position++;
                }
            }
        }

        return position;
    }

    float GetCarProgress(GameObject car)
    {
        LapCount lapCounter = car.GetComponent<LapCount>();
        int lap = lapCounter != null ? lapCounter.currentLap : 0;

        int waypointIndex = 0;
        float distanceToNextWaypoint = 0f;
        float cumulativeDistance = 0f;

        // Get the current waypoint index and calculate distance to the next waypoint
        if (car.GetComponent<AICarController>() != null)
        {
            AICarController aiController = car.GetComponent<AICarController>();
            waypointIndex = aiController.currentWaypointIndex;
        }
        else if (car.GetComponent<CarController>() != null)
        {
            CarController playerController = car.GetComponent<CarController>();
            waypointIndex = playerController.currentWaypointIndex;
        }

        // Get cumulative distance up to current waypoint
        cumulativeDistance = WaypointManager.Instance.cumulativeDistances[waypointIndex];

        // Calculate distance from car to next waypoint
        Transform nextWaypoint = WaypointManager.Instance.waypoints[waypointIndex];
        distanceToNextWaypoint = Vector3.Distance(car.transform.position, nextWaypoint.position);

        // Total progress is the cumulative distance plus the distance covered towards the next waypoint
        float totalProgress = lap * WaypointManager.Instance.totalTrackLength
                              + cumulativeDistance
                              - distanceToNextWaypoint;

        return totalProgress;
    }



    void UpdateLapIndicator()
    {
        LapCount lapCounter = playerCarRigidbody.GetComponent<LapCount>();

        int currentLap = lapCounter != null ? lapCounter.currentLap : 1;
        int totalLaps = lapCounter != null ? lapCounter.totalLaps : 3;

        lapText.text = currentLap.ToString() + " / " + totalLaps.ToString();
    }

    void UpdateRaceTimer()
    {
        float elapsedTime = Time.time - raceStartTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        float seconds = elapsedTime % 60f;

        timerText.text = string.Format("{0}:{1:00.000}", minutes, seconds);
    }
}
