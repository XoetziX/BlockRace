using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPassed
{
    
    private LevelInfoSO.Difficulty levelDifficulty;
    private string mainLevel;
    //private ArrayList subLevel = new ArrayList();
    private List<string> subLevel = new List<string>(); 


   

    public LevelInfoSO.Difficulty LevelDifficulty
    {
        get => levelDifficulty;
        set => levelDifficulty = value;
    }
    public string MainLevel
    {
        get => mainLevel;
        set => mainLevel = value;
    }
    public List<string> SubLevel
    {
        get => subLevel;
        set => subLevel = value;
    }
    public void AddSubLevel(string levelToAdd)
    {
        subLevel.Add(levelToAdd);
    }

    public void DebugOut()
    {
        string tmpsublevels = ""; 
        foreach (string level in subLevel)
        {
            tmpsublevels = tmpsublevels + ", " + level;
        }
        Debug.Log("DEBUG-OUT -> LevelPassed - Diff: " + levelDifficulty + " mainlevel: " + mainLevel + " sublevel: " + tmpsublevels); 
    }
}
