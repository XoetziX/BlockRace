using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_MapLevel : MonoBehaviour
{
    [SerializeField] private Toggle tgl_firstMap;
    [SerializeField] private Toggle tgl_DifficultyEasy;
    [SerializeField] private Toggle tgl_DifficultyMedium;
    [SerializeField] private Toggle tgl_DifficultyHard;
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private LevelInfoSO levelInfoSO;
    [SerializeField] private Sprite level_background_accessable;
    [SerializeField] private Sprite level_background_not_accessable;
    [SerializeField] private Sprite level_background_next;

    async void Start()
    {
        //initialize UI fields - set colors of first toggles
        tgl_firstMap.isOn = false;
        tgl_firstMap.isOn = true;
        Debug.Log("Set Difficulty from SO - levelInfoSO.ChoosenDifficulty: |" + levelInfoSO.ChoosenDifficulty.ToString() + "|");
        //Debug.Log("###### |" + LevelInfoSO.Difficulty.easy + "|");
        if (levelInfoSO.ChoosenDifficulty == LevelInfoSO.Difficulty.easy || levelInfoSO.ChoosenDifficulty.ToString() == "0")
        {
            tgl_DifficultyEasy.isOn = false;
            tgl_DifficultyEasy.isOn = true;
        }
        else if (levelInfoSO.ChoosenDifficulty == LevelInfoSO.Difficulty.medium)
        {
            tgl_DifficultyMedium.isOn = false;
            tgl_DifficultyMedium.isOn = true;
        }
        else if (levelInfoSO.ChoosenDifficulty == LevelInfoSO.Difficulty.hard)
        {
            tgl_DifficultyHard.isOn = false;
            tgl_DifficultyHard.isOn = true;
        }
        else
        {
            Debug.LogError("MainMenu_MapLevel - Start - OH OH - no difficulty");
        }


        Debug.Log("Count vorher: " + playerDataSO.EasyLevelPassed.Count);
        Debug.Log("LOAD Level information from database");
        //playerDataSO.EasyLevelPassed = new List<LevelPassed>();
        await FirebaseManagerGame.instance.LoadLevelPassedAsync();
        Debug.Log("Count nachher: " + playerDataSO.EasyLevelPassed.Count);
        EnableLevelButtons2();
    }

    private void EnableLevelButtons2()
    {
        GameObject myButtonObject;
        Button myButton;
        LevelPassed lvlPassed;
        Debug.Log("EnableLevelButtons2 START ");
        //for each row / main level
        for (int iMainLevel = 1; iMainLevel <= 5; iMainLevel++)
        {
            Debug.Log("HasMainLevel: " + iMainLevel + " BeenPlayed? -> " + playerDataSO.HasMainLevelBeenPlayed(LevelInfoSO.Difficulty.easy, iMainLevel.ToString()));
            //has player already passed at least one level of this mainLevel?
            if (playerDataSO.HasMainLevelBeenPlayed(LevelInfoSO.Difficulty.easy, iMainLevel.ToString()))
            {
                lvlPassed = playerDataSO.EasyLevelPassed[(iMainLevel - 1)];
                Debug.Log("Processing MainLevel -> "); lvlPassed.DebugOut();
                if (lvlPassed.SubLevel.Count < 5)
                {
                    Debug.Log("lvlPassed.SubLevel.Count < 5");
                    Debug.Log("lvlPassed.SubLevel.Count: " + lvlPassed.SubLevel.Count);

                    //already passed level
                    for (int xSubLevel = 1; xSubLevel <= lvlPassed.SubLevel.Count; xSubLevel++)
                    {
                        Debug.Log("ENABLE lvl name = " + "lvl." + lvlPassed.MainLevel + "-" + xSubLevel);
                        myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + xSubLevel);
                        myButton = myButtonObject.GetComponent<Button>();
                        myButton.image.sprite = level_background_accessable;
                        myButton.interactable = true;
                    }

                    //next - not already passed - level
                    Debug.Log("ENABLE NEXT lvl name = " + "lvl." + lvlPassed.MainLevel + "-" + (lvlPassed.SubLevel.Count + 1));
                    myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + (lvlPassed.SubLevel.Count + 1));
                    myButton = myButtonObject.GetComponent<Button>();
                    myButton.image.sprite = level_background_next;
                    myButton.interactable = true;

                    //disable all other level
                    for (int xSubLevel = lvlPassed.SubLevel.Count + 2; xSubLevel < 6; xSubLevel++)
                    {
                        Debug.Log("DISABLE lvl name = " + "lvl." + lvlPassed.MainLevel + "-" + xSubLevel);
                        myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + xSubLevel);
                        myButton = myButtonObject.GetComponent<Button>();
                        myButton.image.sprite = level_background_not_accessable;
                        myButton.interactable = false;
                    }
                }
                else if (lvlPassed.SubLevel.Count == 5)
                {
                    Debug.Log("lvlPassed.SubLevel.Count == 5");
                    for (int x = 1; x <= lvlPassed.SubLevel.Count; x++)
                    {
                        myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + x);
                        myButton = myButtonObject.GetComponent<Button>();
                        myButton.image.sprite = level_background_accessable;
                        myButton.interactable = true;
                    }
                }

            }
            //mainLevel not played yet
            else
            {
                Debug.Log("not lvl passed - Mainlevel: " + iMainLevel);
                myButtonObject = GameObject.Find("lvl." + iMainLevel + "-1");
                myButton = myButtonObject.GetComponent<Button>();
                myButton.image.sprite = level_background_next;
                myButton.interactable = true;

                //disable all other level
                for (int xSubLevel = 2; xSubLevel < 6; xSubLevel++)
                {
                    Debug.Log("DISABLE (mainLevel not played yet) lvl name = " + "lvl." + iMainLevel + "-" + xSubLevel);
                    myButtonObject = GameObject.Find("lvl." + iMainLevel + "-" + xSubLevel);
                    myButton = myButtonObject.GetComponent<Button>();
                    myButton.image.sprite = level_background_not_accessable;
                    myButton.interactable = false;
                }
            }
        }
    }

    public void DeleteLevelProgressOfCurrentUser()
    {
        //Debug.Log("MainMenu_MapLevel - DeleteLevelProgressOfCurrentUser - START ");
        FirebaseManagerGame.instance.DeleteLevelProgressOfUser();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void SetDifficultyEasy(bool clicked)
    {
        if (clicked)
        {
            levelInfoSO.ChoosenDifficulty = LevelInfoSO.Difficulty.easy;
        }
    }
    public void SetDifficultyMedium(bool clicked)
    {
        if (clicked)
            levelInfoSO.ChoosenDifficulty = LevelInfoSO.Difficulty.medium;
    }
    public void SetDifficultyHard(bool clicked)
    {
        if (clicked)
            levelInfoSO.ChoosenDifficulty = LevelInfoSO.Difficulty.hard;
    }
}
