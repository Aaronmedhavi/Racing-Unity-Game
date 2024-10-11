using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text speedText;
    public TMP_Text gearText;
    public TMP_Text timeText;
    public TMP_Text modeText;

    private CarController carController;

    private void Start()
    {
        carController = FindObjectOfType<CarController>();
    }

    private void Update()
    {
        UpdateSpeed();
        UpdateGear();
        UpdateTime();
        UpdateMode();
    }

    private void UpdateSpeed()
    {
        float speed = carController.GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // Convert m/s to km/h
        speedText.text = speed.ToString("F1") + " Km/h";
    }

    private void UpdateGear()
    {
        int gear = carController.GetGear();
        string gearString;

        if (carController.currentDrivingMode == CarController.DrivingMode.Manual)
        {
            gearString = gear switch
            {
                -1 => "R",
                0 => "N",
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                _ => "N"
            };
        }
        else // Automatic mode
        {
            gearString = gear switch
            {
                -1 => "R",
                0 => "N",
                1 => "D1",
                2 => "D2",
                3 => "D3",
                4 => "D4",
                5 => "D5",
                _ => "N"
            };
        }

        gearText.text = "Gear: " + gearString;
    }

    private void UpdateTime()
    {
        float time = Time.time;
        timeText.text = "Time: " + time.ToString("F2");
    }

    private void UpdateMode()
    {
        string mode = carController.currentDrivingMode == CarController.DrivingMode.Manual ? "Manual" : "Automatic";
        modeText.text = "Mode: " + mode;
    }
}
