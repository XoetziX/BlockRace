using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Register : MonoBehaviour
{
    public static MainMenu_Register instance;

    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private PlayerDataSO playerData;

    //Register variables
    [Header("Register")]
    [SerializeField] private TMP_InputField usernameRegisterField;
    [SerializeField] private TMP_InputField emailRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterVerifyField;
    [SerializeField] private TMP_Text warningRegisterText;

 
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
            usernameRegisterField.text = "oetzi";
            emailRegisterField.text = "oetzi@oetzi.de";
            passwordRegisterField.text = "oetzi1!";
            passwordRegisterVerifyField.text = "oetzi1!";

        }
    }


    public void RegisterButton()
    {
        RegisterNewUser(); 
    }
    public async void RegisterNewUser()
    {
        warningRegisterText.text = ""; //if previous register failed
        warningRegisterText.color = Color.red;

        if (usernameRegisterField.text == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (!DoesUsernameContainValidChars(usernameRegisterField.text))
        {
            //If the username does not contain allow characters
            warningRegisterText.text = "Invalid characters - allowed are: Aa123";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else if (await FirebaseManagerAuth.instance.CheckIfUsernameExists(usernameRegisterField.text))
        {
            //Debug.Log($"FirebaseManagerAuth - CheckIfUsernameExists - Username already exists: {usernameRegisterField.text}");
            warningRegisterText.text = "Player name already exists";
        }
        else
        {
            Debug.Log($"MainMenu_Register - RegisterAsyc - Username is available. >> {usernameRegisterField.text} . Going to register in Firebase now");
            string resultOfRegistration = await FirebaseManagerAuth.instance.RegisterAsync(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text); 
            if (resultOfRegistration == "registration_successful")
            {
                warningRegisterText.color = Color.green;
                warningRegisterText.text = "registration successful";
            }
            else if (resultOfRegistration == "registration_failed")
            {
                warningRegisterText.text = "registration failed for unknown reason -> hit the dev";
            }
            else
            {
                warningRegisterText.text = resultOfRegistration;
            }

            //Lambda Expression = public void Transmission(string returnValue) { warningText = returnValue };
            //In the FirebaseManager - Register happens: Execute Transmission("I am a warning text")
            //-> this sets the warningText of (UIManager - RegisterButton) to the warningText of the (Firebase - Register) Method
            //StartCoroutine(FirebaseManagerAuth.instance.Register(emailRegisterField.text,
            //                                                passwordRegisterField.text,
            //                                                passwordRegisterVerifyField.text,
            //                                                usernameRegisterField.text,
            //                                                SetWarningRegisterText));
        }

        
    }

    private bool DoesUsernameContainValidChars(string text)
    {
        // Definieren Sie einen regulären Ausdruck, der Groß- und Kleinbuchstaben sowie Zahlen akzeptiert
        string pattern = "^[a-zA-Z0-9]*$";

        // Verwenden Sie die IsMatch-Methode, um zu prüfen, ob der Text den regulären Ausdruck erfüllt
        return Regex.IsMatch(text, pattern);
    }

    public void CheckIfPlayerNameExists()
    {
        FirebaseManagerAuth.instance.CheckIfUsernameExists(usernameRegisterField.text);
    }

    private void SetWarningRegisterText(string returnValue)
    {
        string warningText = returnValue;
        warningRegisterText.text = warningText;
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu_Login");
    }

    internal void ShowLoginScreen()
    {
        SceneManager.LoadScene("MainMenu_Login");
    }
}
