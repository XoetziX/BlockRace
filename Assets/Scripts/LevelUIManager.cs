using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private PlayerDataSO playerData;

    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    [SerializeField] private LevelInfoSO levelInfo;

    [SerializeField] private Stopwatch stopWatch;
    [SerializeField] private CountdownController countdownController;
    [SerializeField] private HighscoreController highscoreController;

    [Header("UI Objects")]
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Text txt_Distance;
    [SerializeField] private Text txt_ForwardSpeed;
    [SerializeField] private Text txt_LevelName;

    private bool gameOver = false;


    private void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();

        //TODO add a cool way to extract the relevant infos from the scene/level name (e. g. replace "." with " ")
        levelInfo.LevelName = SceneManager.GetActiveScene().name;
        txt_LevelName.text = levelInfo.LevelName;

    }

    

    void Update()
    {
        if (!gameOver)
        {
            txt_Distance.text = ("Distanz: " + playerTransform.position.z.ToString("0") + " m");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResumeGame();
        }
        ////show speed - * 3,6 for km/h
        //float tmpSpeed = playerRigidbody.velocity.magnitude * 3.6f;
        //CurrentSpeed = tmpSpeed.ToString();
        //txt_CurrentSpeed.text = "Speed: " + tmpSpeed.ToString("0");

        //show speed - * 3,6 for km/h - Project = only consider forward Vector
        Vector3 forwardVelocity = Vector3.Project(playerRigidbody.velocity, playerTransform.forward);
        float forwardSpeed = forwardVelocity.magnitude * 3.6f;
        txt_ForwardSpeed.text = "Geschwindigkeit: " + forwardSpeed.ToString("0") + " km/h";
    }

    private void PauseResumeGame()
    {
        if (gameSettings.GameIsPaused)
        {
            gameSettings.ResumeGame = true;
            HidePauseMenuUI();
        }
        else
        {
            gameSettings.PauseGame = true;
            ShowPauseMenuUI();
        }
    }

    public void MenuClicked()
    {
        PauseResumeGame();
    }
    public void ResumeGame()
    {
        HidePauseMenuUI();
        gameSettings.ResumeGame = true;
    }

    public void StartCountdown()
    {
        countdownController.StartLevelCountdown();
    }
    public void StartStopWatch()
    {
        stopWatch.StartStopwatch();
    }
    public void StopStopWatch()
    {
        stopWatch.StopStopwatch();
    }

    internal void AddHighscore()
    {
       StartCoroutine(highscoreController.AddHighScore(playerData.PlayerName, stopWatch.Timer));
    }

    private void ShowPauseMenuUI()
    {
        pauseMenuUI.SetActive(true);
    }
    private void HidePauseMenuUI()
    {
        pauseMenuUI.SetActive(false);
    }
    internal void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    internal void ShowLevelCompleteUI()
    {
        completeLevelUI.SetActive(true);
    }


    public void SetGameOver()
    {
        gameOver = true;
    }
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").ToString());
        SceneManager.LoadScene("MainMenu_Start");
    }
    public void QuitGame()
    {
        gameSettings.QuitGame = true;
    }
}
