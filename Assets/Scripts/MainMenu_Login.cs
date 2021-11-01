using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Login : MonoBehaviour
{
    public bool testModeActive;
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
        if (testModeActive)
        {
            emailLoginField.text = "oetzi@oetzi.de";
            passwordLoginField.text = "oetzi1!";
        }
    }


    public void LoginButton()
    {
        warningLoginText.text = ""; //if previous register failed
        //Call the login coroutine passing the email and password
        StartCoroutine(FirebaseManagerAuth.instance.Login(emailLoginField.text, passwordLoginField.text, SetWarningLoginText, SetInfoLoginText));
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


    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
    }

   
}
