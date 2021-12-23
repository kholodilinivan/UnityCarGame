using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointControl : MonoBehaviour
{
    public CarControl carcontrol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            // print("Hit checkpoint: " + other.GetComponent<Checkpoint>().checkpointNum);
            carcontrol.CheckpointHit(other.GetComponent<Checkpoint>().checkpointNum);
        }
    }
}
