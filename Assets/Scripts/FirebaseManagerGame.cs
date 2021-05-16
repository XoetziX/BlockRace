using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections;
using System;

public class FirebaseManagerGame : MonoBehaviour
{
    public static FirebaseManagerGame instance;
    [SerializeField] private PlayerDataSO playerData;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
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
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Initializing DB");

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

    public IEnumerator UpdateUsernameDatabase(string _username)
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
