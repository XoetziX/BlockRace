using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameSettings.PauseGame){Resume();}
            else{PauseGameTime(); ShowPauseUI(); }
        }

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

    }

    public void PauseGameTime()
    {
        Time.timeScale = 0f;
        gameSettings.PauseGame = false;
        levelUIManager.StopStopWatch();
    }
    public void ShowPauseUI()
    {
        levelUIManager.ShowPauseMenuUI();
    }

    public void Resume()
    {
        levelUIManager.HidePauseMenuUI();
        levelUIManager.StartStopWatch();
        Time.timeScale = 1f;
        gameSettings.ResumeGame = false;
    }


    public void CompleteLevel()
    {
        levelUIManager.SetGameOver();
        levelUIManager.ShowLevelCompleteUI();
        levelUIManager.StopStopWatch();
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

}
