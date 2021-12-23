using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;
    public Checkpoint[] allCheckpoints;
    public int totalLaps = 5;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].checkpointNum = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
