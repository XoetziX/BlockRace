using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;    

    //private float restartDelay = 3f;
    private bool gameHasEnded = false; //Avoid multiple "gameOver" events, e. g. by multiple collisions

    public enum Difficulty{easy=2000,medium=3000,hard=4000}
    private Difficulty difficulty = Difficulty.medium;
    private LevelUIManager levelUIManager;
    private void OnDestroy()
    {
        LogHelper.DebugMe("GAME MANAGER DESTROYED");
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "GameManager";
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("Game Manager - instance - NOT there yet");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Game Manager - Instanz - exists already");
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        if (levelUIManager == null)
        {
            levelUIManager = FindObjectOfType<LevelUIManager>();
            Debug.Log("Game Manager - start - LevelUIManager NOT there yet ######");
        }
        else
        {
            Debug.Log("Game Manager - start - LevelUIManager exists already");
        }

    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CompleteLevel()
    {
        levelUIManager.SetGameOver();
        levelUIManager.ShowLevelCompleteUI();
    }

    public void GameOver()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            levelUIManager.SetGameOver();
            levelUIManager.ShowGameOverUI();
            Debug.Log("Game Over");
        }
    }

    public void Restart()
    {
        gameHasEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetGameDifficulty(Difficulty newDif) => difficulty = newDif;
    public Difficulty getGameDifficulty() => difficulty;

}
