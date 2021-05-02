using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private LevelInfo _levelInfo;
    [SerializeField] private CountdownController countdownController;
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Text txt_Distance;
    [SerializeField] private Text txt_CurrentSpeed;
    [SerializeField] private Text txt_ForwardSpeed;
    [SerializeField] private Text txt_LevelName;

    private bool gameOver = false;

    public string CurrentSpeed = "5"; 

    private void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        _levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        
        txt_LevelName.text = _levelInfo.LevelName;

    }


    void Update()
    {
        if (!gameOver)
        {
            txt_Distance.text = (playerTransform.position.z.ToString("0") + " m");
        }

        ////show speed - * 3,6 for km/h
        //float tmpSpeed = playerRigidbody.velocity.magnitude * 3.6f;
        //CurrentSpeed = tmpSpeed.ToString();
        //txt_CurrentSpeed.text = "Speed: " + tmpSpeed.ToString("0");

        //show speed - * 3,6 for km/h - Project = only consider forward Vector
        Vector3 forwardVelocity = Vector3.Project(playerRigidbody.velocity, playerTransform.forward);
        float forwardSpeed = forwardVelocity.magnitude * 3.6f;
        txt_ForwardSpeed.text = "Forward Speed: " + forwardSpeed.ToString("0");
    }
    public void StartCountdown()
    {
        countdownController.StartLevelCountdown();
    }
    internal void ShowPauseMenuUI()
    {
        pauseMenuUI.SetActive(true);
    }
    internal void HidePauseMenuUI()
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

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").ToString());
        SceneManager.LoadScene(0);
    }

    public void SetGameOver()
    {
        gameOver = true;
    }
}
