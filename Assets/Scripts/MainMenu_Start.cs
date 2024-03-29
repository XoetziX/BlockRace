using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerDataSO;

public class MainMenu_Start : MonoBehaviour
{
    public bool testModeActive;
    public static MainMenu_Start instance;

    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private PlayerDataSO playerData;

    [Header("Start")]
    [SerializeField] private TMP_Text usernameStartField;
    [SerializeField] private ToggleGroup tg_difficulty;



    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("MainMenu_Start - Instance set!");
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("MainMenu_Start - Instance already exists, destroying object!");
            Destroy(this);
        }


        
    }

    private void Start()
    {
        //Show Username
        usernameStartField.text = playerData.PlayerName;

    }

    public void StartGameButton()
    {
        SceneManager.LoadScene("MainMenu_Map-Level");
    }
    public void StartCleanLevel()
    {
        SceneManager.LoadScene("CleanLevel");
    }
    public void StartLongLevel()
    {
        SceneManager.LoadScene("aLongLevel");
    }
    public void StartFogLevel()
    {
        SceneManager.LoadScene("aFogTestingLevel");
    }
    public void QuitGameButton()
    {
        gameSettings.QuitGame = true;
    }
    public void SignOutButton()
    {
        Debug.Log("SignOutButton hit");
        FirebaseManagerAuth.instance.SignOut();
        SceneManager.LoadScene("MainMenu_Login");
    }



    
    public void TestSortList()
    {
        PlayerHighscore p1 = new PlayerHighscore("o1", 1.1f);
        PlayerHighscore p2 = new PlayerHighscore("o2", 2.2f);
        PlayerHighscore p3 = new PlayerHighscore("o3", 3.3f);
        List<PlayerHighscore> highscores1 = new List<PlayerHighscore>();
        List<PlayerHighscore> highscores2 = new List<PlayerHighscore>();

        highscores1.Add(p1);
        highscores1.Add(p3);
        highscores1.Add(p2);
        highscores2.Add(p2);
        highscores2.Add(p3);
        highscores2.Add(p1);

        highscores1.Sort(SortByTime);
        highscores2.Sort(SortByTime);

        Debug.LogWarning("List 1");
        foreach (var item in highscores1)
        {
            item.DebugOut();
        }
        Debug.LogWarning("List 2");
        foreach (var item in highscores2)
        {
            item.DebugOut();
        }

        /////////
        //highscores1.Add(p1);
        //highscores1.Add(p2);
        //highscores2.Add(p2);
        //highscores2.Add(p1);

        //highscores1.Sort(SortByTime);
        //highscores2.Sort(SortByTime);

        //Debug.LogWarning("List 1");
        //foreach (var item in highscores1)
        //{
        //    item.DebugOut();
        //}
        //Debug.LogWarning("List 2");
        //foreach (var item in highscores2)
        //{
        //    item.DebugOut();
        //}

        //////////////
        //Debug.LogWarning("p1, p2: -1");
        //Debug.Log(SortByTime(p1, p2));
        //Debug.LogWarning("p2, p1: 1");
        //Debug.Log(SortByTime(p2, p1));


    }

    private int SortByTime(PlayerHighscore p1, PlayerHighscore p2)
    {
        if (p1.Time < p2.Time)
        {
            return -1;
        }
        else if (p1.Time > p2.Time)
        {
            return 1;
        }
        return 0;
    }
}
