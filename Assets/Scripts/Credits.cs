using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameSettingsSO gameSettings;
    public void Quit()
    {
        gameSettings.QuitGame = true;
    }
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").ToString());
        SceneManager.LoadScene(0);
    }
}
