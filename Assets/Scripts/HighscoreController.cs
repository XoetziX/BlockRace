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

    //[SerializeField] private HighscoreSO highscoresOfThisLevel;
    //[SerializeField] private List<PlayerHighscore> playerHSList;
    [SerializeField] private List<PlayerHighscore> highscoresDB;
    [SerializeField] private LevelInfoSO levelInfo;
    [SerializeField] private GameSettingsSO gameSettings;

    PlayerHighscore currentPlayerHighscore;
    private bool newTopHighscore;
    private bool inTop10;
    private bool notInTop10;

    private void Start()
    {
        
    }

    public void AddHighScore(string playerName, float time)
    {
        currentPlayerHighscore = new PlayerHighscore(playerName, time);
        Debug.LogWarning("WAIT UNTIL DATABASE RESULT IS THERE");
        LoadHighscores();
        
        //reset values
        newTopHighscore = false; inTop10 = false; notInTop10 = false; 

        //wenn liste leer ist einfach hinzufügen -> neuer Nr 1 Score
        //wenn liste noch nicht voll ist -> sortieren, prüfen ob 1. -> ggf. neue Nr 1 Score, neue zeit hinzufügen
        //wenn liste voll (maxNrOfHS):
            //Liste sortieren
            //ist neue zeit < als 1. zeit -> neue Nr 1 Highscore 
            //ist neue zeit > als maxNrOfHS -> kein Top 10 Platz erreicht
            //else neue zeit der liste hinzufügen, diese sortieren und letzte rauschmeißen
        if (highscoresDB.Count == 0)
        {
            highscoresDB.Add(currentPlayerHighscore);
            newTopHighscore = true;
        }
        //else if (highscoresDB.Count < highscoresOfThisLevel.MaxNrOfHS)
        else if (highscoresDB.Count < gameSettings.MaxNrOfHS)
        {
            highscoresDB.Sort(SortByTime);
            CheckIfNewTopScore(currentPlayerHighscore);
            highscoresDB.Add(currentPlayerHighscore);
        }
        else
        {
            highscoresDB.Sort(SortByTime);
            CheckIfNewTopScore(currentPlayerHighscore);
            CheckIfInTop10(currentPlayerHighscore);
            highscoresDB.RemoveAt(0);
            highscoresDB.Add(currentPlayerHighscore);
        }
        highscoresDB.Sort(SortByTime);

        
        SaveHighscoresDB();
        ShowHighScores();
    }

    private void CheckIfInTop10(PlayerHighscore ph)
    {
        if (ph.Time < highscoresDB[highscoresDB.Count - 1].Time)
            inTop10 = true;
    }

    private void CheckIfNewTopScore(PlayerHighscore ph)
    {
        //Debug.Log("TIME COMPARE: " + ph.Time + " < " + playerHSList[0].Time);

        if (ph.Time < highscoresDB[0].Time)
        {
            newTopHighscore = true;
        }
    }

    private void LoadHighscores()
    {
        highscoresDB = FirebaseManagerGame.instance.LoadHighscoresOfLevel(levelInfo.LevelName);
        //Debug.Log("HighscoreController - LoadHighscores - count: " + highscoresDB.Count);
    }
    private void SaveHighscoresDB()
    {
        Debug.Log("SaveHighscoresDB - highscoresDB count: " + highscoresDB.Count);
        StartCoroutine(FirebaseManagerGame.instance.SaveHighscores(highscoresDB, levelInfo.LevelName));
    }

    private void ShowHighScores()
    {
        txt_yourTime.text = currentPlayerHighscore.TimeFormatted;
        if (newTopHighscore)
            lbl_newHS.gameObject.SetActive(true);
        else
            lbl_newHS.gameObject.SetActive(false);
            
        for (int i = 0; i < highscoresDB.Count; i++)
        {
            txt_places_names[i].text = highscoresDB[i].PlayerName;
            txt_places_times[i].text = highscoresDB[i].TimeFormatted;
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
