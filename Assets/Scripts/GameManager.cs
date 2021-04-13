using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //private float restartDelay = 3f;
    private bool gameHasEnded = false;
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }
    private Difficulty difficulty = Difficulty.medium;

    public LevelUIManager levelUIManager;
    public void CompleteLevel()
    {
        levelUIManager.setGameOver();
        levelUIManager.showLevelCompleteUI();
    }

    public void GameOver()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            levelUIManager.setGameOver();
            levelUIManager.showGameOverUI();
            Debug.Log("Game Over");

        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
