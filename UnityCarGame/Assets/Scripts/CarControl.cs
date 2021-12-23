using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float maxSpeed = 30;
    public Rigidbody RB;
    float speedInput;
    public float forwardAccel = 10f, reverseAccel = 5f;
    float turnStrength = 180f;
    float turnInput;
    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;
    public bool grounded;
    public Transform groundRayPoint;
    public LayerMask whatIsGround;
    public float groundRayLength = 1f;
    float dragOnGround;
    public float gravityMode = 10f;
    public AudioSource engineSound;
    int nextCheckpoint;
    public int currentLap = 1;
    public float lapTime, bestLapTime;

    // Start is called before the first frame update
    void Start()
    {
        RB.transform.parent = null;
        dragOnGround = RB.drag;
        UIControl.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps;
    }

    private void Update()
    {
        lapTime += Time.deltaTime;
        var timespan = System.TimeSpan.FromSeconds(lapTime);
        UIControl.instance.currentLapText.text = string.Format("{0:00}m{1:00}.{2:000}s", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
        engineSound.pitch = 1f + (RB.velocity.magnitude / maxSpeed) * 2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        Vector3 normalTarget = Vector3.zero; // basic orientation of teh car

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            normalTarget = hit.normal;
        }

        speedInput = 0f;
        if(Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel; // (0...1)*forwardAccel
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel; // (0...1)*forwardAccel
        }

        turnInput = Input.GetAxis("Horizontal");
        if (grounded && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (RB.velocity.magnitude/maxSpeed), 0));
        }

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);

        // rotate the car to match the normal
        if (grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
        }

        // accelerate only on the ground
        if(grounded)
        {
            RB.AddForce(transform.forward * speedInput * 1000f);
            RB.drag = dragOnGround;
        }       
        else
        {
            RB.drag = 0.1f;
            RB.AddForce(-Vector3.up * gravityMode * 100f);
        }

        transform.position = RB.position;
                
        if (RB.velocity.magnitude > maxSpeed)
        {
            RB.velocity = RB.velocity.normalized * maxSpeed; // 0...55 -> (0...1) * maxSpeed -> 0...30
        }
        // print(RB.velocity.magnitude);
    }

    public void CheckpointHit(int checkpointNum)
    {
        // print(checkpointNum);
        if(checkpointNum == nextCheckpoint)
        {
            nextCheckpoint++;
            if(nextCheckpoint == RaceManager.instance.allCheckpoints.Length)
            {
                nextCheckpoint = 0;
                LapCompleted();
            }
        }
    }

    public void LapCompleted()
    {
        currentLap++;
        if(lapTime<bestLapTime || bestLapTime == 0) // || - or, && - and
        {
            bestLapTime = lapTime;
        }
        lapTime = 0f;
        var timespan = System.TimeSpan.FromSeconds(bestLapTime);
        UIControl.instance.bestLapTimeText.text = string.Format("{0:00}m{1:00}.{2:000}s", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);

        UIControl.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps;
    }
 
}
