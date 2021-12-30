using System;
using UnityEngine;

public class PlayerHighscore
{
    private string playerName;
    private float time;
    private string timeFormatted;
    private DateTime dateTimeHSWasReached;

    public PlayerHighscore(string playerName, float time)
    {
        this.playerName = playerName;
        this.time = time;
        this.timeFormatted = Stopwatch.GimmeTimeFormat(time);
        dateTimeHSWasReached = System.DateTime.Now;
    }

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    public float Time
    {
        get => time;
        set
        {
            time = value;
            timeFormatted = Stopwatch.GimmeTimeFormat(time);
        }
    }
    public string TimeFormatted
    {
        get => timeFormatted;
    }

    public DateTime DateTimeHSreached
    {
        get => dateTimeHSWasReached;
        set => dateTimeHSWasReached = value;
    }

    internal void DebugOut()
    {
        Debug.Log("PlayerName: " + playerName + " | time: " + timeFormatted);
    }
}