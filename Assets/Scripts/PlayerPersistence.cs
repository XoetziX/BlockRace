using System;
using UnityEngine;

public class PlayerPersistence
{
    public static PlayerData GetNewPlayerData()
    {
        return new PlayerData()
        {
            Health = 3,
            BaseForwardForce = 3000f,
            BaseSidewayForce = 50f
        };
    }

    public static PlayerData GetData()
    {
        if (PlayerPrefs.HasKey("Health") == false) {return GetNewPlayerData();}
        return LoadFromPlayerPrefs();
    }

    private static PlayerData LoadFromPlayerPrefs()
    {
        int loadedHealth = PlayerPrefs.GetInt("Health");
        float loadedForwardForce = PlayerPrefs.GetFloat("ForwardForce");
        float loadedSidewayForce = PlayerPrefs.GetFloat("SidewayForce");

        return new PlayerData()
        {
            Health = loadedHealth,
            BaseForwardForce = loadedForwardForce,
            BaseSidewayForce = loadedSidewayForce
        };
    }

    public static void SaveData(PlayerData playerData)
    {
        //Debug.Log("###" + playerData.Health);
        PlayerPrefs.SetInt("Health", playerData.Health);
        PlayerPrefs.SetFloat("ForwardForce", playerData.BaseForwardForce);
        PlayerPrefs.SetFloat("SidewayForce", playerData.BaseSidewayForce);
    }

    
}