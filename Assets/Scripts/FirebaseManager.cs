using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    [SerializeField] private PlayerDataVar playerDataVar;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth fbAuth;
    public FirebaseUser fbUser;
    public DatabaseReference DBreference;

    //User Data variables
    //[Header("UserData")]

    //public TMP_InputField usernameField;
    //public TMP_InputField xpField;
    //public TMP_InputField killsField;
    //public TMP_InputField deathsField;
    //public GameObject scoreElement;
    //public Transform scoreboardContent;

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
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Initializing Firebase Auth + DB");

    }

    //Function for the sign out button
    public void SignOut()
    {
        fbAuth.SignOut();
    }

    ////Function for the save button
    //public void SaveDataButton()
    //{
    //    StartCoroutine(UpdateUsernameAuth(usernameField.text));
    //    StartCoroutine(UpdateUsernameDatabase(usernameField.text));
    //}

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

            playerDataVar.name = fbUser.DisplayName;
            StartCoroutine(LoadPlayerData());

            yield return new WaitForSeconds(1);

            MainMenuUIManager.instance.ShowStartScreen(); // Change to start UI
        }
    }

    public IEnumerator LoadPlayerData()
    {
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>> Load User Data and set PlayerDataVarSO
        Debug.LogWarning("LoadUserData not implemented yet! ToDo: Load User Data and set PlayerDataVarSO");
        //playerDataVar

        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(fbUser.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            //xpField.text = "0";
            //killsField.text = "0";
            //deathsField.text = "0";
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //xpField.text = snapshot.Child("xp").Value.ToString();
            //killsField.text = snapshot.Child("kills").Value.ToString();
            //deathsField.text = snapshot.Child("deaths").Value.ToString();
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

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(fbUser.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    private IEnumerator UpdateXp(int _xp)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(fbUser.UserId).Child("xp").SetValueAsync(_xp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateKills(int _kills)
    {
        //Set the currently logged in user kills
        var DBTask = DBreference.Child("users").Child(fbUser.UserId).Child("kills").SetValueAsync(_kills);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Kills are now updated
        }
    }

    private IEnumerator UpdateDeaths(int _deaths)
    {
        //Set the currently logged in user deaths
        var DBTask = DBreference.Child("users").Child(fbUser.UserId).Child("deaths").SetValueAsync(_deaths);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Deaths are now updated
        }
    }


    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        var DBTask = DBreference.Child("users").OrderByChild("kills").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            ////Data has been retrieved
            //DataSnapshot snapshot = DBTask.Result;

            ////Destroy any existing scoreboard elements
            //foreach (Transform child in scoreboardContent.transform)
            //{
            //    Destroy(child.gameObject);
            //}

            ////Loop through every users UID
            //foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            //{
            //    string username = childSnapshot.Child("username").Value.ToString();
            //    int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
            //    int deaths = int.Parse(childSnapshot.Child("deaths").Value.ToString());
            //    int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());

            //    //Instantiate new scoreboard elements
            //    GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
            //    scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
            //}

            ////Go to scoareboard screen
            //UIManager.instance.ScoreboardScreen();
        }
    }
}
