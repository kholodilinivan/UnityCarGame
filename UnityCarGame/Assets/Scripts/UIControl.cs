using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIControl : MonoBehaviour
{
    public static UIControl instance;
    public TMP_Text currentLapText, bestLapTimeText, lapCounterText, positionText, countDownText,goText, raceResultText;
    public GameObject resultScreen, pauseScreen;
    public bool isPaused;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        if(isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
