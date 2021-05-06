using System;

public class PlayerHighscore
{
    private string playerName;
    private float time;
    private DateTime dateTimeHSreached;

    public PlayerHighscore(string playerName, float time)
    {
        this.playerName = playerName;
        this.time = time;
        dateTimeHSreached = System.DateTime.Now;
    }

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    public float Time
    {
        get => time;
        set => time = value;
    }

    public DateTime DateTimeHSreached
    {
        get => dateTimeHSreached;
        set => dateTimeHSreached = value;
    }

}