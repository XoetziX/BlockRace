using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private LevelInfo _levelInfo;
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text txt_Distance;
    [SerializeField] private Text txt_CurrentSpeed;
    [SerializeField] private Text txt_LevelName;

    private bool gameOver = false;

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

        //show speed
        float tmpSpeed = playerRigidbody.velocity.magnitude * 3.6f;
        txt_CurrentSpeed.text = "Speed: " + tmpSpeed.ToString("0");
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
