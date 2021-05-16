using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfoSO", menuName = "ScriptableObjects/Level Info SO", order = 51)]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] string levelName;

    public string LevelName
    {
        get => levelName;
        set => levelName = value;
    }
}
