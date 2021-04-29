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
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Text txt_Distance;
    [SerializeField] private Text txt_CurrentSpeed;
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

    internal void showPauseMenuUI()
    {
        pauseMenuUI.SetActive(true);
    }
    internal void hidePauseMenuUI()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (!gameOver)
        {
            txt_Distance.text = (playerTransform.position.z.ToString("0") + " m");
        }

        //show speed
        float tmpSpeed = playerRigidbody.velocity.magnitude * 3.6f;
        CurrentSpeed = tmpSpeed.ToString();
        txt_CurrentSpeed.text = "Speed: " + tmpSpeed.ToString("0");
        LocalizedString locString = new LocalizedString();
        //Debug.Log("SPEED" + tmpSpeed.ToString("0"));
    }

    internal void showGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    internal void showLevelCompleteUI()
    {
        completeLevelUI.SetActive(true);
    }

    public void setGameOver()
    {
        gameOver = true;
    }
}
