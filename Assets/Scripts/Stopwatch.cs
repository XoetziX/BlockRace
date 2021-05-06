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
        msec = (int)((timer - (int)timer) * 100);
        sec = (int)(timer % 60);
        min = (int)(timer / 60 % 60);

        //txt_stopWatch.text = min.ToString("0") + ":" + msec.ToString("00") + " min";
        txt_stopWatch.text = string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec);
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
