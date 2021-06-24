using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float timeValue;
    public Text timerText;

    private void Start()
    {
        timeValue = Fuel.CurrentValue * 3; //Fuel is reduced every 3 seconds
    }

    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float millisceconds = timeToDisplay % 1 * 100;

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, millisceconds);
    }
}
