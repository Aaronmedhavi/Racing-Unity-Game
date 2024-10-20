using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public int totalCheckpoints;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCheckpoints();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeCheckpoints()
    {
        // Find all checkpoints in the scene
        GameObject[] checkpointObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
        totalCheckpoints = checkpointObjects.Length;
    }
}
