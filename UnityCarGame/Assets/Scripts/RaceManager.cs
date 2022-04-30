using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;
    public Checkpoint[] allCheckpoints;
    public int totalLaps = 5;

    public CarControl playerCar;
    public List<CarControl> allAICars = new List<CarControl>();
    public int playerPosition;

    public float aiDefaultSpeed = 30f, playerDefaultSpeed = 30f, SpeedVariance = 3.5f, AccelVariance = 0.5f;

    public bool isStarting;
    public float timeBetweenStartCount = 1f;
    float startCounter;
    public int countdownCurrent = 3;
    public bool raceCompleted;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].checkpointNum = i;
        }

        isStarting = true;
        startCounter = timeBetweenStartCount;
        UIControl.instance.countDownText.text = countdownCurrent.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarting)
        {
            startCounter -= Time.deltaTime;
            if(startCounter <= 0)
            {
                countdownCurrent--;
                startCounter = timeBetweenStartCount;
                UIControl.instance.countDownText.text = countdownCurrent.ToString();
                if (countdownCurrent == 0)
                {
                    isStarting = false;
                    UIControl.instance.countDownText.gameObject.SetActive(false);
                    UIControl.instance.goText.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            playerPosition = 1;

            foreach(CarControl aiCar in allAICars)
            {
                if(aiCar.currentLap > playerCar.currentLap)
                {
                    playerPosition++;
                }
                else if (aiCar.currentLap == playerCar.currentLap)
                {
                    if (aiCar.nextCheckpoint > playerCar.nextCheckpoint)
                    {
                        playerPosition++;
                    }
                    else if (aiCar.nextCheckpoint == playerCar.nextCheckpoint)
                    {
                        if (Vector3.Distance(aiCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position) < Vector3.Distance(playerCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position))
                        {
                            playerPosition++;
                        }
                    }
                }
            }
            UIControl.instance.positionText.text = playerPosition + "/" + (allAICars.Count + 1);

            // manage speed variance
            if(playerPosition == 1)
            {
                foreach(CarControl aiCar in allAICars)
                {
                    aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefaultSpeed + SpeedVariance, AccelVariance * Time.deltaTime);
                }
                playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed - SpeedVariance, AccelVariance * Time.deltaTime);
            }
            else
            {
                foreach (CarControl aiCar in allAICars)
                {
                    aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefaultSpeed - SpeedVariance*((float)playerPosition/((float)allAICars.Count+1)), AccelVariance * Time.deltaTime);
                }
                playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed + SpeedVariance * ((float)playerPosition / ((float)allAICars.Count + 1)), AccelVariance * Time.deltaTime);
            }
            }
        
    }

    public void FinishRace()
    {
        raceCompleted = true;

        switch(playerPosition)
        {
            case 1:
                UIControl.instance.raceResultText.text = "You finished 1st";
                break;
            case 2:
                UIControl.instance.raceResultText.text = "You finished 2nd";
                break;
            case 3:
                UIControl.instance.raceResultText.text = "You finished 3rd";
                break;
            default:
                UIControl.instance.raceResultText.text = "You finished" + playerPosition + "th";
                break;
        }

        UIControl.instance.resultScreen.SetActive(true);
    }
}
