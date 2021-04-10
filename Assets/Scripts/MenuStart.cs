using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStart : MonoBehaviour
{
    
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void SetDifficultyToEasy(bool activated)
    {
        if (activated){GameManager.Instance.SetGameDifficulty(GameManager.Difficulty.easy);}
    }
    public void SetDifficultyToMedium(bool activated)
    {
        if (activated) { GameManager.Instance.SetGameDifficulty(GameManager.Difficulty.medium); }
    }
    public void SetDifficultyToHard(bool activated)
    {
        if (activated) { GameManager.Instance.SetGameDifficulty(GameManager.Difficulty.hard); }
    }

}
