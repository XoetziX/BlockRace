using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections;
using System;
using System.Threading.Tasks;

public class FirebaseManagerAuth : MonoBehaviour
{
    public static FirebaseManagerAuth instance;
    [SerializeField] private PlayerDataSO playerData;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth fbAuth;
    public FirebaseUser fbUser;
    //public DatabaseReference DBreference;


    void Awake()
    {
        InitializeFirebase();

        if (instance == null)
        {
            instance = this;
            //Debug.Log("FirebaseManagerAuth - awake - instance was null ");
        }
        else if (instance != null)
        {
            //Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }


    private void InitializeFirebase()
    {
        try
        {
            fbAuth = FirebaseAuth.DefaultInstance;
        }
        catch (Exception e)
        {
            Debug.LogError("Gotcha - AUTH! " + e.Message);
            //throw e;
        }
        if (fbAuth == null)
            Debug.LogError("INIT FirebaseManagerAUTH + fbAuth = null");
        else
            Debug.Log("INIT FirebaseManagerAUTH + sucessfull");
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

    public async Task<string> LoginAwait(string email, string password)
    {
        FirebaseUser user = null;
        try
        {
            //Call the Firebase auth signin function passing the email and password
            user = await fbAuth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log("LoginAwait - Login successful! User-ID: " + user.UserId + " DisplayName: " + user.DisplayName);
            playerData.PlayerName = user.DisplayName;
            playerData.PlayerDBUserId = user.UserId; 
            return null; 
        }
        catch (AggregateException ex)
        {
            FirebaseException authEx = ex.InnerExceptions[0] as FirebaseException;
            if (authEx != null)
            {
                Debug.LogError("Login fehlgeschlagen: " + authEx.Message);
                switch (authEx.ErrorCode)
                {
                    case (int)AuthError.MissingEmail:
                        return ("Missing Email");
                    case (int)AuthError.MissingPassword:
                        return ("Missing Password");
                    case (int)AuthError.WrongPassword:
                        return ("Wrong Password");
                    case (int)AuthError.InvalidEmail:
                        return ("Invalid Email");
                    case (int)AuthError.UserNotFound:
                        return ("Account does not exist");
                }
            }
            else
            {
                Debug.LogError("LoginAwait - unknown error: " + ex.Message);
                return ("unknown login error");
            }
        }

        return null;
    }

    //public IEnumerator Login(string _email, string _password, Action<string> callbackWarningText, Action<string> callbackInfoText)
    //{
    //    //Call the Firebase auth signin function passing the email and password
    //    var LoginTask = fbAuth.SignInWithEmailAndPasswordAsync(_email, _password);
    //    //Wait until the task completes
    //    yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

    //    if (LoginTask.Exception != null)
    //    {
    //        //If there are errors handle them
    //        Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
    //        FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
    //        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

    //        string message = "Login Failed!";
    //        switch (errorCode)
    //        {
    //            case AuthError.MissingEmail:
    //                message = "Missing Email";
    //                break;
    //            case AuthError.MissingPassword:
    //                message = "Missing Password";
    //                break;
    //            case AuthError.WrongPassword:
    //                message = "Wrong Password";
    //                break;
    //            case AuthError.InvalidEmail:
    //                message = "Invalid Email";
    //                break;
    //            case AuthError.UserNotFound:
    //                message = "Account does not exist";
    //                break;
    //        }
    //        callbackWarningText(message);
    //    }
    //    else
    //    {
    //        //User is now logged in
    //        //Now get the result
    //        fbUser = LoginTask.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})", fbUser.DisplayName, fbUser.Email);
    //        callbackInfoText("Logged In");

    //        playerData.PlayerName = fbUser.DisplayName;
    //        playerData.PlayerDBUserId = fbUser.UserId;
    //        //StartCoroutine(FirebaseManagerGame.instance.LoadPlayerData());

    //        yield return new WaitForSeconds(1);

    //        MainMenu_Login.instance.ShowStartScreen(); // Change to start UI
    //    }
    //}


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
                        //MainMenuUIManager.instance.ShowLoginScreen();
                        MainMenu_Register.instance.ShowLoginScreen();
                        callbackWarningText("");
                    }
                }
            }
        }
    }


    /*By default, Firebase Realtime Database rules restrict read and write access to only authenticated users. So, if you try to read data from the database without 
     * authenticating the user first, you may get a permission denied error.
    When you authenticate the user using Firebase Authentication, you get an instance of the FirebaseUser class that represents the currently authenticated user. 
    You can use this user object to check if the user is authenticated and to access the user's information, such as the user ID and email address.
     */
    public async Task<bool> CheckIfDisplayNameAlreadyExists(string displayName)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            // Get a reference to the Firebase Realtime Database
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            // Create a query to check if the display name is already taken
            Query query = reference.Child("users").OrderByChild("displayName").EqualTo(displayName);
            DataSnapshot snapshot = await query.GetValueAsync();
            if (snapshot.HasChildren)
            {
                // Display name is already taken
                // Display an error message or prompt the user to choose a different display name
                return true;
            }
            else
            {
                // Display name is available
                // Proceed with setting the display name for the new user
                return false;
            }
        }
        else
        {
            // User is not authenticated
            // Display an error message or prompt the user to sign in
            return true;
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
