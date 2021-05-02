using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    //public enum Difficulty { easy = 2000, medium = 3000, hard = 4000 }

    private LevelUIManager levelUIManager;

    private void Start()
    {
        levelUIManager = FindObjectOfType<LevelUIManager>();

        //reset values in SO
        _gameSettings.GameHasEnded = false;
        _gameSettings.GameIsPaused = false;

        //ONLY FOR TEST IN ORDER TO START WITHOUT START MENU
        _gameSettings.NewLevelLoaded = true;
        if (_gameSettings.NewLevelLoaded)
        {
            _gameSettings.GameIsPaused = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameSettings.GameIsPaused){Resume();}
            else{PauseGameTime(); ShowPauseUI(); }
        }

        if (_gameSettings.NewLevelLoaded)
        {
            levelUIManager.StartCountdown(); //at the end of the countdown -> _gameSettings.GameIsPaused = false;
            _gameSettings.NewLevelLoaded = false;

        }

        if (_gameSettings.GameIsPaused)
        {
            PauseGameTime();
        }
        else
        {
            Resume();
        }

    }

    public void PauseGameTime()
    {
        Time.timeScale = 0f;
        _gameSettings.GameIsPaused = true;
    }
    public void ShowPauseUI()
    {
        levelUIManager.ShowPauseMenuUI();
    }

    public void Resume()
    {
        levelUIManager.HidePauseMenuUI();
        Time.timeScale = 1f;
        _gameSettings.GameIsPaused = false;
    }


    public void CompleteLevel()
    {
        levelUIManager.SetGameOver();
        levelUIManager.ShowLevelCompleteUI();
    }

    public void GameOver()
    {
        if (_gameSettings.GameHasEnded == false)
        {
            _gameSettings.GameHasEnded = true;
            levelUIManager.SetGameOver();
            levelUIManager.ShowGameOverUI();
            Debug.Log("Game Over");

        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        _gameSettings.NewLevelLoaded = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
