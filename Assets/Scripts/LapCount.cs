using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCount : MonoBehaviour
{
    public int totalLaps = 3;
    private int currentLap = 0;
    private bool hasStarted = false;
    private bool isForward = true;
    public GameObject playerCar;
    private Vector3 lastCheckpointPosition;

    private void Start()
    {
        lastCheckpointPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerDirection = playerCar.transform.forward;
            Vector3 checkpointDirection = transform.forward;
            isForward = Vector3.Dot(playerDirection, checkpointDirection) > 0;
            if (isForward)
            {
                if (!hasStarted)
                {
                    hasStarted = true;
                }
                else
                {
                    currentLap++;
                    if (currentLap < totalLaps)
                    {
                        Debug.Log("Lap " + currentLap + " Completed");
                    }
                    else if (currentLap == totalLaps)
                    {
                        Debug.Log("Race Finished");
                    }
                }
            }
            else
            {
                Debug.Log("Wrong Way");
            }
        }
    }
}
