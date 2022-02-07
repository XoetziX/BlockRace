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
using System.Threading.Tasks;

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
        InitializeFirebase();

        if (instance == null)
        {
            //Debug.Log("FirebaseManagerGame - awake - instance was null");
            instance = this;
        }
        else if (instance != null)
        {
            //Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        Debug.Log("FirebaseManagerGame - instance: " + instance.ToString()) ;
    }


    //async void CheckDependenciesSync()
    //{
    //    var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
    //    if (dependencyStatus == DependencyStatus.Available)
    //    {
    //        // all good, firebase init passed
    //        Debug.Log("Firebase Loaded");
    //        InitializeFirebase();
    //    }
    //    else
    //    {
    //        Debug.Log("Firebase Failed");
    //    }
    //}

    private void InitializeFirebase()
    {
        try
        {
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        catch (Exception e)
        {
            Debug.LogError("Gotcha - GAME! " + e.Message);
            throw e;
        }
        Debug.Log("INIT FirebaseManagerGAME -> DBreference: " + DBreference.ToString());
    }

    public void SavePlayerData()
    {
        StartCoroutine(SavePlayerDataToDB());
    }
    private IEnumerator SavePlayerDataToDB()
    {
        Debug.Log("Start SavePlayerDataToDB - DB-ID: " + playerData.PlayerDBUserId + " Difficulty: " + playerData.ChoosenDifficulty + " name (not stored): " + playerData.PlayerName);
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
        //Debug.Log("SaveHighscores START");
        string jsonNet = JsonConvert.SerializeObject(highscoresDB);
        //Debug.Log("SaveListOfObject " + jsonNet);

        Task DBTask = DBreference.Child("levelHighscores").Child(levelName).SetRawJsonValueAsync(jsonNet);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Highscore is now updated
            Debug.Log("SaveHighscores " + levelName + DBTask.Status);

        }

    }


    //public List<PlayerHighscore> LoadHighscoresOfLevel(string levelName)
    //{
    //    Debug.Log("FireBaseGame - LoadHighscoresOfLevel START");
    //    List<PlayerHighscore> tmpList = new List<PlayerHighscore>();
    //    //IEnumerator cannot return any internal values / variables. Hence, the following way can be used:
    //    //pass a place holder (= returnValue) to the IEnumerator. Within the IEnumerator this placeholder can be set to any value. Outside the Enumerator / here it can be further processed. 
    //    StartCoroutine(GetHighscoresOfLevel(levelName, returnValue =>
    //    {
    //        tmpList = returnValue; 
    //    }));
    //    Debug.Log("LoadHighscoresOfLevel List.count: " + tmpList.Count);
    //    return tmpList;
    //}



    public IEnumerator GetHighscoresOfLevel(string levelName, System.Action<List<PlayerHighscore>> callback)
    {
        Debug.Log("FireBaseGame - getHighscoresOfLevel START");

        List<PlayerHighscore> tmpHighscores = new List<PlayerHighscore>();
        
        Task<DataSnapshot> DBTask = DBreference.Child("levelHighscores").Child(levelName).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null) //No data exists yet
        {
            Debug.Log("getHighscoresOfLevel - NO DATA EXISTS");
        }
        else //Data has been retrieved
        {
            DataSnapshot snapshot = DBTask.Result;
            //Debug.Log("JSON?!? LoadListOfObjects --> " + snapshot.GetRawJsonValue());
            tmpHighscores = JsonConvert.DeserializeObject<List<PlayerHighscore>>(snapshot.GetRawJsonValue());
            Debug.Log("getHighscoresOfLevel - FOUND DATA - count: " + tmpHighscores.Count);
            //foreach (var item in tmpHighscores)
            //{
            //    item.DebugOut();
            //}
            callback(tmpHighscores);
        }
    }

    public List<PlayerHighscore> LoadHighscoresOfLevelSync(string levelName)
    {
        Debug.Log("FireBaseGame - LoadHighscoresOfLevelSync START");

        List<PlayerHighscore> tmpHighscores = new List<PlayerHighscore>();
        //DBreference.Child("levelHighscores").Child(levelName).get(); 

        //    .ContinueWithOnMainThread(DBTask =>
        //{
        //    if (DBTask.IsFaulted)
        //        Debug.Log("OH OH, ERROR");
        //    else if (DBTask.IsCompleted)
        //    {
        //        DataSnapshot snapshot = DBTask.Result;
        //        //Debug.Log("JSON?!? LoadListOfObjects --> " + snapshot.GetRawJsonValue());
        //        tmpHighscores = JsonConvert.DeserializeObject<List<PlayerHighscore>>(snapshot.GetRawJsonValue());

        //        Debug.Log("LoadHighscoresOfLevelSync - FOUND DATA - count: " + tmpHighscores.Count);
        //        foreach (var item in tmpHighscores)
        //        {
        //            item.DebugOut();
        //        }
        //    }
        //});
        //Debug.Log("FireBaseGame - LoadHighscoresOfLevelSync END");



        //DBreference.Child("levelHighscores").Child(levelName).GetValueAsync().ContinueWithOnMainThread(DBTask =>
        //{
        //    if (DBTask.IsFaulted)
        //        Debug.Log("OH OH, ERROR");
        //    else if (DBTask.IsCompleted)
        //    {
        //        DataSnapshot snapshot = DBTask.Result;
        //        //Debug.Log("JSON?!? LoadListOfObjects --> " + snapshot.GetRawJsonValue());
        //        tmpHighscores = JsonConvert.DeserializeObject<List<PlayerHighscore>>(snapshot.GetRawJsonValue());

        //        Debug.Log("LoadHighscoresOfLevelSync - FOUND DATA - count: " + tmpHighscores.Count);
        //        foreach (var item in tmpHighscores)
        //        {
        //            item.DebugOut();
        //        }
        //    }
        //});
        //Debug.Log("FireBaseGame - LoadHighscoresOfLevelSync END");

        return tmpHighscores;
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
