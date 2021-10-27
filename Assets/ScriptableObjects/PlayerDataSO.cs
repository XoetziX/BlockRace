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
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

    [Header("Fixed Values")]
    [SerializeField] private float _baseSidewayForce;

    private void OnEnable()
    {
        choosenDifficulty = Difficulty.hard;
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


}
