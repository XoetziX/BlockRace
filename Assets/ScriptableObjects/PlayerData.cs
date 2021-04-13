using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Player Data", order = 51)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float _baseForwardForce;
    [SerializeField] private float _baseSidewayForce;
    [SerializeField] private GameManager.Difficulty _choosenDifficulty;

    public float BaseForwardForce
    {
        get
        {
            return _baseForwardForce;
        }
    }
    public float BaseSidewayForce
    {
        get
        {
            return _baseSidewayForce;
        }
    }
    public GameManager.Difficulty ChoosenDifficulty
    {
        get
        {
            return _choosenDifficulty;
        }
    }

    public float GetCurrentForwardForce()
    {
        //TODO difficulty einrechnen
        return _baseForwardForce;
    }

    public float getCurrentSidewayForce()
    {
        return _baseSidewayForce;
    }
}
