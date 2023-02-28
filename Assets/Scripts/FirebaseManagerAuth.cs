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

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth fbAuth;
    public FirebaseUser fbUser;
    public DatabaseReference DBreference;

    [Header("Debug")]
    [SerializeField] private bool doDebugImportant;
    [SerializeField] private bool doDebugAdditional;

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
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        catch (Exception e)
        {
            Debug.LogError("Gotcha - AUTH! " + e.Message);
        }
        if (fbAuth == null)
            Debug.LogError("INIT FirebaseManagerAUTH + fbAuth = null");
        else
            Debug.Log("INIT FirebaseManagerAUTH + sucessfull");
    }

    public void CheckIfUserIsAuthenticatedTEST()
    {
        if (fbAuth.CurrentUser != null)
        {
            // a valid token exists for the current user
            Debug.Log("CheckIfUserIsAuthenticated() - FirebaseAuth.DefaultInstance.CurrentUser - VALID: " + fbAuth.CurrentUser.UserId + " DisplayName: " + fbAuth.CurrentUser.DisplayName);
        }
        else
        {
            // no valid token exists
            Debug.Log("CheckIfUserIsAuthenticated() - FirebaseAuth.DefaultInstance.CurrentUser - NOT valid ");
        }
    }

    //Function for the sign out button
    public void SignOut()
    {
        fbAuth.SignOut();
        Debug.Log("Signed out");
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

    public async Task<string> RegisterAsync(string email, string password, string username)
    {
        string message = "registration_successful"; //will be returned if no error occurs

        try
        {
            //Call the Firebase auth signin function passing the email and password and wait for the result
            FirebaseUser fbUser = await fbAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            //User has now been created

            //Create a user profile and set the username
            UserProfile profile = new UserProfile { DisplayName = username };

            //Call the Firebase auth update user profile function passing the profile with the username
            await fbUser.UpdateUserProfileAsync(profile);

            if (doDebugImportant) Debug.Log($"Auth - RegisterAsync - Going to add username: {fbAuth.CurrentUser.DisplayName} and db-id: {fbAuth.CurrentUser.UserId} into usernames_dbid");
            AddUsernameDbId(fbAuth.CurrentUser.DisplayName, fbAuth.CurrentUser.UserId); 

            // Optional: Verify that the profile has been updated
            //FirebaseUser user = fbAuth.CurrentUser;
            Debug.LogWarning("ask ChatGPT how to handle an possible error at this late stage where the user is already created"); 
            //if (user != null && user.DisplayName == username)
            //{
            //    // Profile updated successfully
            //}
            //else
            //{
            //    // Profile update failed
            //}

        }
        catch (AggregateException ex)
        {
            FirebaseException authEx = ex.InnerExceptions[0] as FirebaseException;
            //If there are errors handle them
            Debug.LogError($"Failed to register the new user");
            AuthError errorCode = (AuthError)authEx.ErrorCode;
            message = "registration_failed"; 
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email already in use";
                    break;
            }
        }

        return message; 
    }

    public IEnumerator AddUsernameDbId(string displayName, string userId)
    {
        if (doDebugImportant) Debug.Log($"Auth - AddUsernameDbId - Displayname: {displayName} and userId: {userId}");

        Task DBTask = DBreference.Child("usernames_dbid").Child(displayName).Child(userId).SetValueAsync("bla");

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            if (doDebugImportant) Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            if (doDebugImportant) Debug.Log($"Auth - AddUsernameDbId - Added successfully");
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
                        //MainMenuUIManager.instance.ShowLoginScreen();
                        MainMenu_Register.instance.ShowLoginScreen();
                        callbackWarningText("");
                    }
                }
            }
        }
    }


    public async Task<bool> CheckIfUsernameExists(string username)
    {

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("usernames_dbid");
        DataSnapshot snapshot = await reference.Child(username).GetValueAsync();
        //if (snapshot.Exists)
        //{
        //   Debug.Log($"FirebaseManagerAuth - CheckIfUsernameExists - Username already exists: {snapshot.Key}");            
        //}
        //else
        //{
        //    Debug.Log("FirebaseManagerAuth - CheckIfUsernameExists - Username is available. >> " + username);
        //}
        return snapshot.Exists;
    }


}
