using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerNoLevel : MonoBehaviour
{
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private LevelInfoSO levelInfoSO;

    private void Start()
    {
        //reset values in SO
        gameSettings.GameHasEnded = false;
        gameSettings.PauseGame = false;

    }
    // Update is called once per frame
    void Update()
    {
        //used from outside GameManager in order to avoid reference to GameManager
        if (gameSettings.PauseGame)
        {
            PauseGameTime();
        }
        else if (gameSettings.ResumeGame) //e. g. used by CountDownController
        {
            Resume();
        }

        if (gameSettings.QuitGame)
        {
            QuitGame();
        }

    }

    public void PauseGameTime()
    {
        Time.timeScale = 0f;
        gameSettings.PauseGame = false;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameSettings.ResumeGame = false;
    }

    public void QuitGame()
    {
        gameSettings.QuitGame = false;
        Debug.Log("Quit Game");
        Application.Quit();
    }


    public void LoadThisLevel(string mainLevel, string subLevel)
    {

        try
        {
            levelInfoSO.MainLevel = mainLevel;
            levelInfoSO.SubLevel = subLevel;
            Debug.Log("Going to load level: " + mainLevel + "-" + subLevel);
            SceneManager.LoadScene("Level " + mainLevel + "-" + subLevel);
        }
        catch (System.Exception)
        {
            Debug.Log("Could not load level / scene. ------------->>>>>>>>> TODO: Pop-Up Info");
        }
    }
}
