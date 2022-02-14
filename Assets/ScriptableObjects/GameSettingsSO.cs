using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSettings", menuName = "ScriptableObjects/Game Settings SO", order = 51)]
public class GameSettingsSO : ScriptableObject
{

    [Header("Game stats - var")]
    [SerializeField] private bool newLevelLoaded;                       //set TRUE before loading a new level, in order to allow the game manager to show the countdown first
    [SerializeField] private bool gameHasEnded;
    [SerializeField] private bool pauseGame;
    [SerializeField] private bool gameIsPaused;
    [SerializeField] private bool resumeGame; 
    [SerializeField] private bool quitGame;
    [Header("Game stats - fix")]
    [SerializeField] private float startCountdownDelay;


    [Header("Highscores")]
    [SerializeField] private int maxNrOfHS;


    private void OnEnable()
    {
        //Reset values if not want to use the stored ones from previous play
        maxNrOfHS = 5;
        startCountdownDelay = 0.5f;
    }

    public bool NewLevelLoaded
    {
        get => newLevelLoaded;
        set => newLevelLoaded = value;
    }
    public bool GameHasEnded
    {
        get => gameHasEnded;
        set => gameHasEnded = value;
    }
    public bool PauseGame
    {
        get => pauseGame;
        set => pauseGame = value;
    }
    public bool GameIsPaused
    {
        get => gameIsPaused;
        set => gameIsPaused = value;
    }
    public bool ResumeGame
    {
        get => resumeGame;
        set => resumeGame = value;
    }
    public bool QuitGame
    {
        get => quitGame;
        set => quitGame = value;
    }
    public float StartCountdownDelay
    {
        get => startCountdownDelay;
    }
   
    public int MaxNrOfHS
    {
        get => maxNrOfHS;
    }



}
