using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Login : MonoBehaviour
{
    public static MainMenu_Login instance;

    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private PlayerDataSO playerData;

    //Login variables
    [Header("Login")]
    [SerializeField] private TMP_InputField emailLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;
    [SerializeField] private TMP_Text warningLoginText;
    [SerializeField] private TMP_Text confirmLoginText; //currently warningLoginText used for positiv AND negativ response


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
        if (gameSettings.Testmode)
        {
            emailLoginField.text = "oetzi@oetzi.de";
            passwordLoginField.text = "oetzi1!";
        }
    }


    public async void LoginButton()
    {
        warningLoginText.text = ""; //if previous register failed
        string loginError = await FirebaseManagerAuth.instance.LoginAwait(emailLoginField.text, passwordLoginField.text); 
        if (loginError != null)
        {
            warningLoginText.text = loginError; 
        }
        else
        {
            Debug.LogWarning("TODO - check wether the following method is called before the login (and therewith the User-DB-ID) has been loaded");
            confirmLoginText.text = "Login successful";
            await FirebaseManagerGame.instance.LoadLevelPassedAsync();
            Debug.Log("Level should have been loaded, going to load map-level scene");
            ShowStartScreen();
        }
    }

    private void SetWarningLoginText(string returnText)
    {
        string warningText = returnText;
        warningLoginText.text = warningText;
    }
    private void SetInfoLoginText(string returnText)
    {
        confirmLoginText.text = returnText;
    }

    public void ShowStartScreen()
    {
        SceneManager.LoadScene("MainMenu_Start");
    }

    public void ShowRegisterScreen()
    {
        SceneManager.LoadScene("MainMenu_Register");
    }

    public void FillLoginFields()
    {
        emailLoginField.text = "oetzi@oetzi.de";
        passwordLoginField.text = "oetzi1!";
    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
    }

   
}
