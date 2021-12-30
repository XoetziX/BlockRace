using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Start : MonoBehaviour
{
    public bool testModeActive;
    public static MainMenu_Start instance;

    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private PlayerDataSO playerData;

    [Header("Start")]
    [SerializeField] private TMP_Text usernameStartField;



    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("MainMenu_Start - Instance set!");
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("MainMenu_Start - Instance already exists, destroying object!");
            Destroy(this);
        }

        usernameStartField.text = playerData.PlayerName;
    }


    public void StartGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartCleanLevel()
    {
        SceneManager.LoadScene("CleanLevel");
    }
    public void StartLongLevel()
    {
        SceneManager.LoadScene("aLongLevel");
    }
    public void QuitGameButton()
    {
        gameSettings.QuitGame = true;
    }
    public void SignOutButton()
    {
        Debug.Log("SignOutButton hit");
        //FirebaseManagerRegLogin.instance.SignOut();
        Debug.LogWarning("MainMenu_Start -> SignOutButton not fully implemented");
        FirebaseManagerGame.instance.SavePlayerData();
        //ShowLoginScreen();
    }

    public void SignOutButton2()
    {
        
    }


    public void SetDifficultyEasy(bool clicked)
    {
        if (clicked)
        {
            playerData.ChoosenDifficulty = PlayerDataSO.Difficulty.easy;
        }
    }
    public void SetDifficultyMedium(bool clicked)
    {
        if (clicked)
            playerData.ChoosenDifficulty = PlayerDataSO.Difficulty.medium;
    }
    public void SetDifficultyHard(bool clicked)
    {
        if (clicked)
            playerData.ChoosenDifficulty = PlayerDataSO.Difficulty.hard;
    }
   
}
