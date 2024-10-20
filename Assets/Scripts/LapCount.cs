using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCount : MonoBehaviour
{
    public int currentLap = 0;
    public int totalLaps = 3; 
    public int checkpointIndex = 0; 

    private bool raceFinished = false;

    private Collider carCollider;

    void Start()
    {
        carCollider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            if (IsCrossingFinishLineCorrectly())
            {
                if (checkpointIndex == CheckpointManager.Instance.totalCheckpoints)
                {
                    currentLap++;
                    checkpointIndex = 0;

                    if (currentLap > totalLaps)
                    {
                        raceFinished = true;
                        RaceManager.Instance.CarFinishedRace(this.gameObject);
                    }
                }
            }
        }
        else if (other.CompareTag("Checkpoint"))
        {
            int checkpointNumber = other.GetComponent<Checkpoint>().checkpointNumber;
            if (checkpointNumber == checkpointIndex)
            {
                checkpointIndex++;
            }
        }
    }

    private bool IsCrossingFinishLineCorrectly()
    {
        float dotProduct = Vector3.Dot(transform.forward, Vector3.forward);
        return dotProduct < 0;
    }
}
