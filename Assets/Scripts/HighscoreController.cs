using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour
{
    [SerializeField] private List<Text> txt_places_names;
    //[SerializeField] private Text[] txt_places_times;
    [SerializeField] private List<Text> txt_places_times;

    [SerializeField] private HighscoreSO highscoresOfThisLevel;
    [SerializeField] private List<PlayerHighscore> playerHSList;

    public void AddHighScore(string playerName, float time)
    {
        PlayerHighscore ph = new PlayerHighscore(playerName, time);
        playerHSList = highscoresOfThisLevel.Highscores;

        if (playerHSList.Count == 0)
        {
            playerHSList.Add(ph);
        }
        else
        {
            //wenn liste leer ist einfach hinzufügen -> neuer Nr 1 Score
            //wenn liste noch nicht voll ist -> sortieren, prüfen ob 1. -> ggf. neue Nr 1 Score, neue zeit hinzufügen
            //wenn liste voll (maxNrOfHS):
            //Liste sortieren
            //ist neue zeit < als 1. zeit -> neue Nr 1 Highscore 
            //ist neue zeit > als maxNrOfHS -> kein Top 10 Platz erreicht
            //else neue zeit der liste hinzufügen, diese sortieren und letzte rauschmeißen
            playerHSList.Sort(SortByTime);

            //else
            playerHSList.Sort(SortByTime);
            if (playerHSList.Count >= highscoresOfThisLevel.MaxNrOfHS)
            {
                playerHSList.RemoveAt(0);

            }
            playerHSList.Add(ph);
            playerHSList.Sort(SortByTime);
        }




        highscoresOfThisLevel.Highscores = playerHSList;
        LoadAndShowHighScores();
    }

    public void LoadAndShowHighScores()
    {
        List<PlayerHighscore> playerHighscores = highscoresOfThisLevel.Highscores;
        PlayerHighscore tmpPH; 

        for (int i = 0; i < playerHighscores.Count; i++)
        {
            tmpPH = playerHighscores[i];
            txt_places_names[i].text = tmpPH.PlayerName;
            txt_places_times[i].text = tmpPH.Time.ToString();
        }

    }

    private int SortByTime(PlayerHighscore p1, PlayerHighscore p2)
    {
        if (p1.Time < p2.Time)
        {
            return -1;
        }
        else if (p1.Time > p2.Time)
        {
            return 1;
        }
        return 0;
    }
}
