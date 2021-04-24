using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Player Data VAR", order = 51)]
public class PlayerDataVar : ScriptableObject
{
    [SerializeField] private GameManager.Difficulty _choosenDifficulty;

    //[SerializeField] private Transform transform; 
    //[SerializeField] private Transform transform; 

    private void OnEnable()
    {
        //Reset values if not want to use the stored ones from previous play
    }

    public GameManager.Difficulty ChoosenDifficulty
    {
        get
        {
            return _choosenDifficulty;
        }
    }


}
