using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    public Text txt_Distance;
    public Text txt_currentSpeed;
    private bool gameOver = false;

    private void Awake()
    {
        Debug.Log(">>> LevelUIManager - awake - " + gameOverUI);
    }
    private void Start()
    {
        Debug.Log(">>> LevelUIManager - start - " + gameOverUI);
        //ShowGameOverUI();
    }
    private void OnDestroy()
    {
        LogHelper.DebugMe("LEVELUIMANAGER DESTROYED");
    }

    void Update()
    {
        Debug.Log(">>> LevelUIManager - UPDATE - " + gameOverUI);
        if (!gameOver)
        {
            txt_Distance.text = (player.transform.position.z.ToString("0") + " m");
        }

        //show speed
        float tmpSpeed = player.rigidBody.velocity.magnitude * 3.6f;
        txt_currentSpeed.text = "Speed: " + tmpSpeed.ToString("0");
    }

    public void ShowGameOverUI()
    {
        Debug.Log(">>> LevelUIManager - ShowGameOverUI - " + gameOverUI);
        gameOverUI.SetActive(true);
    }

    public void ShowLevelCompleteUI()
    {
        completeLevelUI.SetActive(true);
    }

    public void SetGameOver()
    {
        gameOver = true;
    }
}
