using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    //Screen object variables
    public GameObject loginUI;
    //[SerializeField] private GameObject loginUI;
    public GameObject registerUI;
    public GameObject startUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Functions to change the login screen UI

    public void ClearScreen() //Turn off all screens
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        startUI.SetActive(false);
    }

    public void ShowLoginScreen() //Back button
    {
        ClearScreen();
        loginUI.SetActive(true);
    }
    public void ShowRegisterScreen() // Regester button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void ShowStartScreen() //Logged in
    {
        ClearScreen();
        startUI.SetActive(true);
    }

}
