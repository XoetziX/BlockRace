using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //private float restartDelay = 3f;
    private bool gameHasEnded = false;
    private bool gameIsPaused;
    public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

    public LevelUIManager levelUIManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused){Resume();}
            else{Pause();}
        }
    }

    public void Pause()
    {
        levelUIManager.showPauseMenuUI();
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        levelUIManager.hidePauseMenuUI();
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

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
