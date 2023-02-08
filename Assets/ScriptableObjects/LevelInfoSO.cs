using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfoSO", menuName = "ScriptableObjects/Level Info SO", order = 51)]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] string levelName;
    [SerializeField] string mainLevel;
    [SerializeField] string subLevel;
    [SerializeField] PlayerDataSO.Difficulty difficulty; 

    //TODO: Compose levelname from mainlevel and sublevel
    public string LevelName
    {
        get => levelName;
        set => levelName = value;
    }
    public string MainLevel { get => mainLevel; set => mainLevel = value; }
    public string SubLevel { get => subLevel; set => subLevel = value; }
    public PlayerDataSO.Difficulty Difficulty { get => difficulty; set => difficulty = value; }
}
