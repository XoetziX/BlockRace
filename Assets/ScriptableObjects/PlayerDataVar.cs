using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Player Data VAR", order = 51)]
public class PlayerDataVar : ScriptableObject
{
    [SerializeField] private string playerName;
    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

}
