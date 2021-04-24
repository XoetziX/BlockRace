using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Player Data FIX", order = 51)]
public class PlayerDataFix : ScriptableObject
{
    [SerializeField] private float _baseForwardForce = 3000;
    [SerializeField] private float _baseSidewayForce = 50;

    //[SerializeField] private Transform transform; 
    //[SerializeField] private Transform transform; 

    private void OnEnable()
    {
        //Reset values if not want to use the stored ones from previous play
    }

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

}
