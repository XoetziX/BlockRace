using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour
{
    [SerializeField] private List<Text> txt_places_names;
    [SerializeField] private List<Text> txt_places_times;
    [SerializeField] private Text txt_yourTime;
    [SerializeField] private Text lbl_newHS;

    [SerializeField] private HighscoreSO highscoresOfThisLevel;
    [SerializeField] private List<PlayerHighscore> playerHSList;
    [SerializeField] private LevelInfo levelInfo;

    PlayerHighscore currentPlayerHighscore;
    private bool newTopHighscore;
    private bool inTop10;
    private bool notInTop10;

    private void Start()
    {
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();

    }

    public void AddHighScore(string playerName, float time)
    {
        currentPlayerHighscore = new PlayerHighscore(playerName, time);
        playerHSList = highscoresOfThisLevel.Highscores;
        
        //reset values
        newTopHighscore = false; inTop10 = false; notInTop10 = false; 

        //wenn liste leer ist einfach hinzufügen -> neuer Nr 1 Score
        //wenn liste noch nicht voll ist -> sortieren, prüfen ob 1. -> ggf. neue Nr 1 Score, neue zeit hinzufügen
        //wenn liste voll (maxNrOfHS):
            //Liste sortieren
            //ist neue zeit < als 1. zeit -> neue Nr 1 Highscore 
            //ist neue zeit > als maxNrOfHS -> kein Top 10 Platz erreicht
            //else neue zeit der liste hinzufügen, diese sortieren und letzte rauschmeißen
        if (playerHSList.Count == 0)
        {
            playerHSList.Add(currentPlayerHighscore);
            newTopHighscore = true;
        }
        else if (playerHSList.Count < highscoresOfThisLevel.MaxNrOfHS)
        {
            playerHSList.Sort(SortByTime);
            CheckIfNewTopScore(currentPlayerHighscore);
            playerHSList.Add(currentPlayerHighscore);
        }
        else
        {
            playerHSList.Sort(SortByTime);
            CheckIfNewTopScore(currentPlayerHighscore);
            CheckIfInTop10(currentPlayerHighscore);
            playerHSList.RemoveAt(0);
            playerHSList.Add(currentPlayerHighscore);
        }
        playerHSList.Sort(SortByTime);

        //write back to SO
        highscoresOfThisLevel.Highscores = playerHSList;
        //highscoresOfThisLevel.printHS();
        LoadAndShowHighScores();
    }

    private void CheckIfInTop10(PlayerHighscore ph)
    {
        if (ph.Time < playerHSList[playerHSList.Count - 1].Time)
            inTop10 = true;
    }

    private void CheckIfNewTopScore(PlayerHighscore ph)
    {
        //Debug.Log("TIME COMPARE: " + ph.Time + " < " + playerHSList[0].Time);

        if (ph.Time < playerHSList[0].Time)
        {
            newTopHighscore = true;
        }
    }

    private void LoadAndShowHighScores()
    {
        List<PlayerHighscore> playerHighscores = highscoresOfThisLevel.Highscores;
        PlayerHighscore tmpPH;

        txt_yourTime.text = currentPlayerHighscore.TimeFormatted;
        if (newTopHighscore)
            lbl_newHS.gameObject.SetActive(true);
            

        for (int i = 0; i < playerHighscores.Count; i++)
        {
            tmpPH = playerHighscores[i];
            txt_places_names[i].text = tmpPH.PlayerName;
            txt_places_times[i].text = tmpPH.TimeFormatted;
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
