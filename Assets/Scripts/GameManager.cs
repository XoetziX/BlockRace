using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private LevelInfoSO levelInfo;

    private LevelUIManager levelUIManager;

    private void Start()
    {
        //ONLY FOR TEST IN ORDER TO START WITHOUT START MENU
        gameSettings.NewLevelLoaded = true;

        levelUIManager = FindObjectOfType<LevelUIManager>();

        //reset values in SO
        gameSettings.GameHasEnded = false;
        gameSettings.PauseGame = false;

        if (gameSettings.NewLevelLoaded)
        {
            gameSettings.PauseGame = true; //could call PauseGameTime() directly
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameSettings.NewLevelLoaded)
        {
            levelUIManager.StartCountdown(); //at the end of the countdown -> _gameSettings.GameIsPaused = false;
            gameSettings.NewLevelLoaded = false;

        }

        //used from outside GameManager in order to avoid reference to GameManager
        if (gameSettings.PauseGame)
        {
            PauseGameTime();
        }
        else if (gameSettings.ResumeGame) //e. g. used by CountDownController
        {
            Resume();
        }

        if (gameSettings.QuitGame)
        {
            QuitGame();
        }

    }


    public void PauseGameTime()
    {
        Time.timeScale = 0f;
        gameSettings.PauseGame = false;
        gameSettings.GameIsPaused = true;
        levelUIManager.StopStopWatch();
    }
        
    private void Resume()
    {
        levelUIManager.StartStopWatch();
        Time.timeScale = 1f;
        gameSettings.ResumeGame = false;
        gameSettings.GameIsPaused = false;
    }


    public void CompleteLevel()
    {
        SaveLevelPassed();

        levelUIManager.SetGameOver();
        levelUIManager.ShowLevelCompleteUI();
        levelUIManager.StopStopWatch();
        levelUIManager.AddHighscore();
    }

    public void SaveLevelPassed()
    {
        Debug.Log("SaveLevelPassed - Difficulty: " + levelInfo.ChoosenDifficulty.ToString() + " - Main: " + levelInfo.MainLevel + " - Sub: " + levelInfo.SubLevel);
        StartCoroutine(FirebaseManagerGame.instance.SaveLevelPassed(levelInfo.ChoosenDifficulty.ToString(), levelInfo.MainLevel, levelInfo.SubLevel));
    }

    public void GameOver()
    {
        if (gameSettings.GameHasEnded == false)
        {
            gameSettings.GameHasEnded = true;
            levelUIManager.SetGameOver();
            levelUIManager.ShowGameOverUI();
            levelUIManager.StopStopWatch();
            Debug.Log("Game Over");

        }
    }

    public void Restart()
    {
        gameSettings.NewLevelLoaded = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        gameSettings.NewLevelLoaded = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame()
    {
        gameSettings.QuitGame = false;
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
