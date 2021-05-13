using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public bool testModeActive;
    public static MainMenuUIManager instance;

    //Login variables
    [Header("Login")]
    [SerializeField] private TMP_InputField emailLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;
    [SerializeField] private TMP_Text warningLoginText;
    [SerializeField] private TMP_Text confirmLoginText; //currently warningLoginText used for positiv AND negativ response

    //Register variables
    [Header("Register")]
    [SerializeField] private TMP_InputField usernameRegisterField;
    [SerializeField] private TMP_InputField emailRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterVerifyField;
    [SerializeField] private TMP_Text warningRegisterText;

    //Screen object variables
    [Header("Screen objects")]
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerUI;
    [SerializeField] private GameObject startUI;

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
            usernameRegisterField.text = "oetzi";
            emailRegisterField.text = "oetzi@oetzi.de";
            passwordRegisterField.text = "oetzi1!";
            passwordRegisterVerifyField.text = "oetzi1!";

            emailLoginField.text = "oetzi@oetzi.de";
            passwordLoginField.text = "oetzi1!";
        }
    }

    
    public void RegisterButton()
    {
        warningRegisterText.text = ""; //if previous register failed
        //Lambda Expression = public void Transmission(string returnValue) { warningText = returnValue };
        //In the FirebaseManager - Register happens: Execute Transmission("I am a warning text")
        //-> this sets the warningText of (UIManager - RegisterButton) to the warningText of the (Firebase - Register) Method
        StartCoroutine(FirebaseManager.instance.Register(emailRegisterField.text,
                                                        passwordRegisterField.text,
                                                        passwordRegisterVerifyField.text,
                                                        usernameRegisterField.text,
                                                        SetWarningRegisterText));
        ClearRegisterFields();
        ClearLoginFields();
    }
    private void SetWarningRegisterText(string returnValue)
    {
        string warningText = returnValue;
        warningRegisterText.text = warningText;
    }

    public void LoginButton()
    {
        warningLoginText.text = ""; //if previous register failed
        //Call the login coroutine passing the email and password
        StartCoroutine(FirebaseManager.instance.Login(emailLoginField.text, passwordLoginField.text, SetWarningLoginText));
    }
    private void SetWarningLoginText(string returnText)
    {
        string warningText = returnText;
        warningLoginText.text = warningText;
    }

    public void SignOutButton()
    {
        FirebaseManager.instance.SignOut();
        ShowLoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
    }


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
    public void ShowRegisterScreen() // Register button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void ShowStartScreen() //Logged in
    {
        ClearScreen();
        startUI.SetActive(true);
    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }
}
