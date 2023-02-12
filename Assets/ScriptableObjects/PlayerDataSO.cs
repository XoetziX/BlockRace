using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "ScriptableObjects/Player Data", order = 51)]
public class PlayerDataSO : ScriptableObject
{
    [Header("Variable Values")]
    [SerializeField] private string playerName;
    [SerializeField] private string playerDBUserId;
    [SerializeField] private Difficulty choosenDifficulty;
    [SerializeField] private List<LevelPassed> easyLevelPassed = new List<LevelPassed>();
    [SerializeField] private List<LevelPassed> mediumLevelPassed = new List<LevelPassed>();
    [SerializeField] private List<LevelPassed> hardLevelPassed = new List<LevelPassed>();
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

    [Header("Fixed Values")]
    [SerializeField] private float _baseSidewayForce;
    //[SerializeField] private ArrayList<LevelPassed> passedLevelList;

    private void OnEnable()
    {
        //choosenDifficulty = Difficulty.hard;
        Debug.LogWarning("PlayerDataSO OnEnable - difficulty set correctly? also for new players? -> " + choosenDifficulty);
        _baseSidewayForce = 50;
    }

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    public string PlayerDBUserId
    {
        get => playerDBUserId;
        set => playerDBUserId = value;
    }

    public bool HasMainLevelBeenPlayed(Difficulty diffToCheck, string mainLevelToCheck)
    {
        if (diffToCheck == Difficulty.easy)
        {
            foreach (LevelPassed lvlPassed in easyLevelPassed)
            {
                if (lvlPassed.MainLevel == mainLevelToCheck)
                {
                    return true; 
                }
            }

        }
        else if (diffToCheck == Difficulty.medium)
        {
            foreach (LevelPassed lvlPassed in mediumLevelPassed)
            {
                if (lvlPassed.MainLevel == mainLevelToCheck)
                {
                    return true;
                }
            }

        }
        else if (diffToCheck == Difficulty.hard)
        {
            foreach (LevelPassed lvlPassed in hardLevelPassed)
            {
                if (lvlPassed.MainLevel == mainLevelToCheck)
                {
                    return true;
                }
            }

        }
        return false; 
    }

    public Difficulty ChoosenDifficulty
    {
        get => choosenDifficulty;
        set => choosenDifficulty = value;
    }
    public float BaseSidewayForce
    {
        get
        {
            return _baseSidewayForce;
        }
    }

    public List<LevelPassed> EasyLevelPassed { get => easyLevelPassed; set => easyLevelPassed = value; }
    public List<LevelPassed> MediumLevelPassed { get => mediumLevelPassed; set => mediumLevelPassed = value; }
    public List<LevelPassed> HardLevelPassed { get => hardLevelPassed; set => hardLevelPassed = value; }
    public void DebugOutLevelPassedLists()
    {
        Debug.Log("DEBUG OUT - PlayerDataSO - DebugOutLevelPassedLists: ");
        foreach (LevelPassed lvl in easyLevelPassed) { lvl.DebugOut(); }
        foreach (LevelPassed lvl in mediumLevelPassed) { lvl.DebugOut(); }
        foreach (LevelPassed lvl in hardLevelPassed) { lvl.DebugOut(); }
    }
}
