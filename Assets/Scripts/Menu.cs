using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameSettings gameSettings;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        gameSettings.QuitGame = true;
    }
    public void SetDifficultyEasy()
    {
        gameSettings.ChoosenDifficulty = GameSettings.Difficulty.easy;
    }
    public void SetDifficultyMedium()
    {
        gameSettings.ChoosenDifficulty = GameSettings.Difficulty.medium;
    }
    public void SetDifficultyHard()
    {
        gameSettings.ChoosenDifficulty = GameSettings.Difficulty.hard;
    }
}
