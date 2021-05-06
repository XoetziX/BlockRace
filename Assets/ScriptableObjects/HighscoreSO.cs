using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHighscoreSO", menuName = "Highscore SO", order = 51)]
public class HighscoreSO : ScriptableObject
{
    [SerializeField] private List<PlayerHighscore> highscores = new List<PlayerHighscore>();
    [SerializeField] private int maxNrOfHS = 3;

    //public void Add(PlayerHighscore playerHS)
    //{
    //    //TODO einsortieren und letzten rauskicken

    //    highscores.Sort(SortByTime);
        

    //    if (highscores.Count >= maxNrOfHS)
    //    {
    //        highscores.RemoveAt(0);

    //    }
    //    highscores.Add(playerHS);
    //}

    public List<PlayerHighscore> Highscores
    {
        get => highscores;
        set => highscores = value;
    }
    public int MaxNrOfHS
    {
        get => maxNrOfHS;
    }

    private void printHS()
    {
        foreach (PlayerHighscore ph in highscores)
        {
            Debug.Log(">>> Name: " + ph.PlayerName + " >>> Time: " + ph.Time);
        }
    }
}
