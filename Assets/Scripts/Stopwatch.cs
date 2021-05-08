using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    float timer;
    float msec;
    float sec;
    float min;
    [SerializeField] Text txt_stopWatch;
    bool doCalculation;


    // Start is called before the first frame update
    void Start()
    {
        doCalculation = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (doCalculation)
        {
            StopWatchCalculation();
        }
    }

    void StopWatchCalculation()
    {
        timer += Time.deltaTime;
        //example: timer = 1.3375 | int(timer) = 1 | (int)((timer - (int)timer) * 1000) = 337
        msec = (int)((timer - (int)timer) * 1000);
        sec = (int)(timer % 60);
        min = (int)(timer / 60 % 60);

        txt_stopWatch.text = string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec);
    }

    public static string GimmeTimeFormat(float time)
    {
        float tmpMsec = (int)((time - (int)time) * 1000);
        float tmpSec = (int)(time % 60);
        float tmpMin = (int)(time / 60 % 60);

        return string.Format("{0:00}:{1:00}:{2:00}", tmpMin, tmpSec, tmpMsec);
    }

    public void StartStopwatch()
    {
        doCalculation = true;
    }
    public void StopStopwatch()
    {
        doCalculation = false;
    }
    public void ResetStopwatch()
    {
        timer = 0;
        txt_stopWatch.text = "0:00 min";
    }

    public float Timer
    {
        get => timer;
    }
}
