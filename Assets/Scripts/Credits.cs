using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").ToString());
        SceneManager.LoadScene(0);
    }
}
