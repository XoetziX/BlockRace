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

    //public IEnumerator SavePlayerDataToDB()
    //{
    //    Debug.Log("Start SavePlayerDataToDB - DB-ID: " + playerData.PlayerDBUserId + " Difficulty: " + playerData.ChoosenDifficulty + " name (not stored): " + playerData.PlayerName);
    //    //Set the currently logged in user username in the database

    //    var DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("username").SetValueAsync(playerData.PlayerName); //PlayerName        
    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    //    if (DBTask.Exception != null)
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");

    //    DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("choosenDifficulty").SetValueAsync(playerData.ChoosenDifficulty.ToString()); //ChoosenDifficulty
    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    //    if (DBTask.Exception != null)
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //}

    public void SavePlayerDataToDBWithoutTask()
    {
        //without analyzing the task result
        DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("username").SetValueAsync(playerData.PlayerName); //PlayerName       
        DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("choosenDifficulty").SetValueAsync(playerData.ChoosenDifficulty.ToString()); //ChoosenDifficulty

    }

    /*
     * loads all player information and stores them directly into the PlayerDataSO (hence, no return value neccessary)
     */
    public IEnumerator LoadAllPlayerData()
    {
        yield return LoadLevelPassed(); 
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
            //Debug.Log("SaveHighscores " + levelName + DBTask.Status);
        }

    }



    public IEnumerator GetHighscoresOfLevel(string levelName, System.Action<List<PlayerHighscore>> callback)
    {

        List<PlayerHighscore> tmpHighscores = new List<PlayerHighscore>();
        
        Task<DataSnapshot> DBTask = DBreference.Child("levelHighscores").Child(levelName).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null) //No data exists yet
        {
            Debug.Log("FirebaseManagerGame - getHighscoresOfLevel - NO DATA EXISTS");
        }
        else //Data has been retrieved
        {
            DataSnapshot snapshot = DBTask.Result;
            //Debug.Log("JSON?!? LoadListOfObjects --> " + snapshot.GetRawJsonValue());
            tmpHighscores = JsonConvert.DeserializeObject<List<PlayerHighscore>>(snapshot.GetRawJsonValue());
            Debug.Log("FirebaseManagerGame - getHighscoresOfLevel - FOUND DATA - count: " + tmpHighscores.Count);
            //foreach (var item in tmpHighscores)
            //{
            //    item.DebugOut();
            //}
            callback(tmpHighscores);
        }
    }

    public IEnumerator SaveLevelPassed(string difficulty, string mainLevel, string subLevel)
    {
        Debug.Log("SaveLevelPassed START - DB User ID: " + playerData.PlayerDBUserId + " - Diff: " + difficulty + " - mainLevel: " + mainLevel + " - subLevel: " + subLevel);

        Task DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("levelPassed").Child(difficulty).Child(mainLevel).Child(subLevel).SetValueAsync("passed");

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Highscore is now updated
            Debug.Log("SaveLevelPassed END " + mainLevel + "-" + subLevel + " " + DBTask.Status);
        }

    }

    /*
     * loads every information below the node "levelPassed" and stores them directly into the PlayerDataSO (hence, no return value neccessary)
     */
    public IEnumerator LoadLevelPassed()
    {
        Debug.Log("GetLevelPassed START - DB User ID: " + playerData.PlayerDBUserId);
        List <LevelPassed> levelPassed = new List<LevelPassed>();

        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(playerData.PlayerDBUserId).Child("levelPassed").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null) //No data exists yet
        {
            Debug.Log("FirebaseManagerGame - GetLevelPassed - NO DATA EXISTS");
        }
        else //Data has been retrieved
        {
            DataSnapshot snapshot = DBTask.Result;

            List<LevelPassed> easyLevelPassed = new List<LevelPassed>();
            List<LevelPassed> mediumLevelPassed = new List<LevelPassed>();
            List<LevelPassed> hardLevelPassed = new List<LevelPassed>();
            
            if (snapshot.HasChildren)//snapshot = levelPassed --> children = easy, medium, hard
            {
                foreach (DataSnapshot difficultySnapshot in snapshot.Children) //easy, medium, hard
                {
                    //Debug.Log("difficultySnapshot Key: " + difficultySnapshot.Key); 
                    if (difficultySnapshot.Key == PlayerDataSO.Difficulty.easy.ToString() & difficultySnapshot.HasChildren)
                    {
                        foreach (DataSnapshot mainLevelSnapshot in difficultySnapshot.Children)//1 main level
                        {
                            //Debug.Log("mainLevelSnapshot Key: " + mainLevelSnapshot.Key); 
                            LevelPassed tmpLevelPassed = new LevelPassed();
                            tmpLevelPassed.LevelDifficulty = PlayerDataSO.Difficulty.easy; 
                            tmpLevelPassed.MainLevel = mainLevelSnapshot.Key;

                            foreach (DataSnapshot subLevelSnapshot in mainLevelSnapshot.Children)//1,2,3,4,5 sub level
                            {
                                //Debug.Log("subLevelSnapshot Key: " + subLevelSnapshot.Key);
                                tmpLevelPassed.AddSubLevel(subLevelSnapshot.Key);
                            }
                            //tmpLevelPassed.DebugOut();
                            easyLevelPassed.Add(tmpLevelPassed);
                        }

                    }
                    else if (difficultySnapshot.Key == PlayerDataSO.Difficulty.medium.ToString() & difficultySnapshot.HasChildren)
                    {
                        foreach (DataSnapshot mainLevelSnapshot in difficultySnapshot.Children)//1 main level
                        {
                            //Debug.Log("mainLevelSnapshot Key: " + mainLevelSnapshot.Key);
                            LevelPassed tmpLevelPassed = new LevelPassed();
                            tmpLevelPassed.LevelDifficulty = PlayerDataSO.Difficulty.medium;
                            tmpLevelPassed.MainLevel = mainLevelSnapshot.Key;

                            foreach (DataSnapshot subLevelSnapshot in mainLevelSnapshot.Children)//1,2,3,4,5 sub level
                            {
                                //Debug.Log("subLevelSnapshot Key: " + subLevelSnapshot.Key);
                                tmpLevelPassed.AddSubLevel(subLevelSnapshot.Key);
                            }
                            //tmpLevelPassed.DebugOut();
                            mediumLevelPassed.Add(tmpLevelPassed);
                        }

                    }
                    else if (difficultySnapshot.Key == PlayerDataSO.Difficulty.hard.ToString() & difficultySnapshot.HasChildren)
                    {
                        foreach (DataSnapshot mainLevelSnapshot in difficultySnapshot.Children)//1 main level
                        {
                            //Debug.Log("mainLevelSnapshot Key: " + mainLevelSnapshot.Key);
                            LevelPassed tmpLevelPassed = new LevelPassed();
                            tmpLevelPassed.LevelDifficulty = PlayerDataSO.Difficulty.hard;
                            tmpLevelPassed.MainLevel = mainLevelSnapshot.Key;

                            foreach (DataSnapshot subLevelSnapshot in mainLevelSnapshot.Children)//1,2,3,4,5 sub level
                            {
                                //Debug.Log("subLevelSnapshot Key: " + subLevelSnapshot.Key);
                                tmpLevelPassed.AddSubLevel(subLevelSnapshot.Key);
                            }
                            //tmpLevelPassed.DebugOut();
                            hardLevelPassed.Add(tmpLevelPassed);
                        }

                    }
                    //foreach(LevelPassed lvl in easyLevelPassed)
                    //{
                    //    lvl.DebugOut();
                    //}
                    //foreach (LevelPassed lvl in mediumLevelPassed)
                    //{
                    //    lvl.DebugOut();
                    //}
                    //foreach (LevelPassed lvl in hardLevelPassed)
                    //{
                    //    lvl.DebugOut();
                    //}
                }
                playerData.EasyLevelPassed = easyLevelPassed;
                playerData.MediumLevelPassed = mediumLevelPassed;
                playerData.HardLevelPassed = hardLevelPassed;
                playerData.DebugOutLevelPassedLists();

            }


        }
    }
}
