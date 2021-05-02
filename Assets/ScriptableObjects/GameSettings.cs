using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSettings", menuName = "Game Settings SO", order = 51)]
public class GameSettings : ScriptableObject
{
    [SerializeField] private bool newLevelLoaded;                       //set TRUE before loading a new level, in order to allow the game manager to show the countdown first
    [SerializeField] private bool gameHasEnded;
    [SerializeField] private bool gameIsPaused; 
    [SerializeField] private Difficulty _choosenDifficulty;
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

    public bool NewLevelLoaded { get; set; }
    public bool GameHasEnded { get; set; }
    public bool GameIsPaused { get; set; }
    public Difficulty ChoosenDifficulty { get; set; }

    private void OnEnable()
    {
        Debug.Log("_choosenDifficulty: " + _choosenDifficulty);
        _choosenDifficulty = Difficulty.medium;
        //Reset values if not want to use the stored ones from previous play
    }



}
