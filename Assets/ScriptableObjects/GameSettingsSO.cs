using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSettings", menuName = "Game Settings SO", order = 51)]
public class GameSettingsSO : ScriptableObject
{
    [SerializeField] private bool newLevelLoaded;                       //set TRUE before loading a new level, in order to allow the game manager to show the countdown first
    [SerializeField] private bool gameHasEnded;
    [SerializeField] private bool pauseGame; 
    [SerializeField] private bool resumeGame; 
    [SerializeField] private bool quitGame; 
    [SerializeField] private Difficulty choosenDifficulty;
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

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
    public Difficulty ChoosenDifficulty
    {
        get => choosenDifficulty;
        set => choosenDifficulty = value;
    }

    private void OnEnable()
    {
        //Debug.Log("_choosenDifficulty: " + choosenDifficulty);
        choosenDifficulty = Difficulty.medium;
        //Reset values if not want to use the stored ones from previous play
    }



}
