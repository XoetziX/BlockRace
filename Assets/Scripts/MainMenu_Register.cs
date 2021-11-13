using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Register : MonoBehaviour
{
    public bool testModeActive;
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
        if (testModeActive)
        {
            usernameRegisterField.text = "oetzi";
            emailRegisterField.text = "oetzi@oetzi.de";
            passwordRegisterField.text = "oetzi1!";
            passwordRegisterVerifyField.text = "oetzi1!";

        }
    }

    
    public void RegisterButton()
    {
        warningRegisterText.text = ""; //if previous register failed
        //Lambda Expression = public void Transmission(string returnValue) { warningText = returnValue };
        //In the FirebaseManager - Register happens: Execute Transmission("I am a warning text")
        //-> this sets the warningText of (UIManager - RegisterButton) to the warningText of the (Firebase - Register) Method
        StartCoroutine(FirebaseManagerAuth.instance.Register(emailRegisterField.text,
                                                        passwordRegisterField.text,
                                                        passwordRegisterVerifyField.text,
                                                        usernameRegisterField.text,
                                                        SetWarningRegisterText));
        
    }
    private void SetWarningRegisterText(string returnValue)
    {
        string warningText = returnValue;
        warningRegisterText.text = warningText;
    }

    public void QuitGameButton()
    {
        gameSettings.QuitGame = true;
    }
  


}
