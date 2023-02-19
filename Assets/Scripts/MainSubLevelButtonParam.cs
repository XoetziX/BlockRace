using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSubLevelButtonParam : MonoBehaviour
{
    public string mainLevel;
    public string subLevel;
    private GameManagerNoLevel gameManagerNoLevel; 

    public void LoadThisLevel()
    {
        Debug.Log("MainSubLevelButtonParam using FindObject - check wether it has impact on the performance.");
        gameManagerNoLevel = GameObject.FindObjectOfType<GameManagerNoLevel>();
        if (gameManagerNoLevel != null)
        {
            try
            {
                gameManagerNoLevel.LoadThisLevel(mainLevel, subLevel);
            }
            catch (System.Exception)
            {
                Debug.LogError("MainSubLevelButtonParam - main and/or sublevel not set");
            }
        }
        else
        {
            Debug.LogWarning("Could not find GameManagerNoLevel");
        }
    }

}
