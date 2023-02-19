using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfoSO", menuName = "ScriptableObjects/Level Info SO", order = 51)]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] string levelName;
    [SerializeField] string mainLevel;
    [SerializeField] string subLevel;
    [SerializeField] Difficulty choosenDifficulty;
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

    //TODO: Compose levelname from mainlevel and sublevel
    public string LevelName
    {
        get => levelName;
        set => levelName = value;
    }
    public string MainLevel { get => mainLevel; set => mainLevel = value; }
    public string SubLevel { get => subLevel; set => subLevel = value; }
    public Difficulty ChoosenDifficulty { get => choosenDifficulty; set => choosenDifficulty = value; }
}
