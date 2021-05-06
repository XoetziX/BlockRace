using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private PlayerDataVar playerDataVar;
    [SerializeField] private GameObject inp_playerName;


    private void Start()
    {
        inp_playerName.GetComponent<Text>().text = playerDataVar.PlayerName;
    }

    public void StartGame()
    {
        playerDataVar.PlayerName = inp_playerName.GetComponent<Text>().text;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        gameSettings.QuitGame = true;
    }
    public void SetDifficultyEasy()
    {
        gameSettings.ChoosenDifficulty = GameSettings.Difficulty.easy;
    }
    public void SetDifficultyMedium()
    {
        gameSettings.ChoosenDifficulty = GameSettings.Difficulty.medium;
    }
    public void SetDifficultyHard()
    {
        gameSettings.ChoosenDifficulty = GameSettings.Difficulty.hard;
    }
}
