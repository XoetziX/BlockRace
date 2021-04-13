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

    void Update()
    {
        if (!gameOver)
        {
            txt_Distance.text = (player.transform.position.z.ToString("0") + " m");
        }

        //show speed
        float tmpSpeed = player.rigidBody.velocity.magnitude * 3.6f;
        txt_currentSpeed.text = "Speed: " + tmpSpeed.ToString("0");
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
