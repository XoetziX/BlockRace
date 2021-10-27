using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Firebase.Extensions;

public class FirebaseManagerGame : MonoBehaviour
{
    public static FirebaseManagerGame instance;
    [SerializeField] private PlayerDataSO playerData;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    //public FirebaseUser fbUser; // instead --> playerData.PlayerDBUserId
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
        try
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
                Debug.Log("FirebaseManagerGame - awake - instance was null");
                instance = this;
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }

        }
        catch (Exception e)
        {
            Debug.Log("CATCHED - FirebaseGame - awake -> " + e.Message);
            //throw;
        }
        
    }

    private void InitializeFirebase()
    {
        try
        {
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        catch (Exception e)
        {
            Debug.Log("Gotcha - GAME! " + e.Message);
            throw e;
        }
        Debug.Log("INIT FirebaseManagerGAME + DBreference: " + DBreference.ToString());
    }

    public void SavePlayerData()
    {
        Debug.Log("Start SavePlayerData");
        StartCoroutine(SavePlayerDataToDB());
    }
    private IEnumerator SavePlayerDataToDB()
    {
        Debug.Log("Start SavePlayerDataToDB");
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).SetValueAsync(playerData.ChoosenDifficulty.ToString());
        
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

    public IEnumerator LoadPlayerData()
    {
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>> Load User Data and set PlayerDataVarSO
        Debug.LogWarning("LoadUserData not implemented yet! ToDo: Load User Data and set PlayerDataVarSO");
        //playerDataVar

        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).GetValueAsync();

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
        var DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("username").SetValueAsync(_username);

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

    public IEnumerator SaveHighscores(List<PlayerHighscore> highscoresDB, String levelName)
    {
        string jsonNet = JsonConvert.SerializeObject(highscoresDB);
        Debug.Log("SaveListOfObject " + jsonNet);
        //DBreference.Child("LevelHighscores").SetRawJsonValueAsync(jsonNet);



        
        //var DBTask = DBreference.Child("LevelHighscores").Child(levelName).SetRawJsonValueAsync(jsonNet);
        var DBTask = DBreference.Child("LevelHighscores").SetValueAsync("123 test");

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Highscore is now updated
            Debug.Log("Done - " + DBTask.Status);
                
        }
    }

    internal List<PlayerHighscore> getHighscoresOfLevel(string levelName)
    {
        
        List<PlayerHighscore> tmpListe = new List<PlayerHighscore>();


        tmpListe.Add(new PlayerHighscore("dummy1", 1.11f));
        Debug.LogWarning("FireBaseGame - getHighscoresOfLevel DUMMY MODE");
        return tmpListe;
    }

    private IEnumerator UpdateXp(int _xp)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("xp").SetValueAsync(_xp);

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

}
