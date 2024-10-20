using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    private List<GameObject> finishedCars = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CarFinishedRace(GameObject car)
    {
        finishedCars.Add(car);
        Debug.Log(car.name + " has finished the race in position " + finishedCars.Count);
    }
}
