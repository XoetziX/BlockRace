using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections;
using System;

public class FirebaseManagerRegLogin : MonoBehaviour
{
    public static FirebaseManagerRegLogin instance;
    [SerializeField] private PlayerDataSO playerData;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth fbAuth;
    public FirebaseUser fbUser;
    //public DatabaseReference DBreference;


    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

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

    private void InitializeFirebase()
    {
        //Set the authentication instance object
        fbAuth = FirebaseAuth.DefaultInstance;
        //DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Initializing Firebase Auth + DB");

    }

    //Function for the sign out button
    public void SignOut()
    {
        fbAuth.SignOut();
    }

    //Function for the save button
    public void SaveDataButton(string _newUserName)
    {
        StartCoroutine(UpdateUsernameAuth(_newUserName));
        StartCoroutine(FirebaseManagerGame.instance.UpdateUsernameDatabase(_newUserName));
    }

    public IEnumerator Login(string _email, string _password, Action<string> callbackWarningText, Action<string> callbackInfoText)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = fbAuth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            callbackWarningText(message);
        }
        else
        {
            //User is now logged in
            //Now get the result
            fbUser = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", fbUser.DisplayName, fbUser.Email);
            callbackInfoText("Logged In");

            playerData.PlayerName = fbUser.DisplayName;
            StartCoroutine(FirebaseManagerGame.instance.LoadPlayerData());

            yield return new WaitForSeconds(1);

            MainMenuUIManager.instance.ShowStartScreen(); // Change to start UI
        }
    }


    public IEnumerator Register(string _email, string _password, string _confirmPassword, string _username, Action<string> callbackWarningText)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            callbackWarningText("Missing Username");
        }
        else if (_password != _confirmPassword)
        {
            //If the password does not match show a warning
            callbackWarningText("Password Does Not Match!");
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = fbAuth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                callbackWarningText(message);
            }
            else
            {
                //User has now been created
                //Now get the result
                fbUser = RegisterTask.Result;

                if (fbUser != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = fbUser.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        callbackWarningText("Username Set Failed!");
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        MainMenuUIManager.instance.ShowLoginScreen();
                        callbackWarningText("");
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = fbUser.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }
}