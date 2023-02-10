using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_MapLevel : MonoBehaviour
{
    [SerializeField] private Toggle tgl_firstMap;
    [SerializeField] private Toggle tgl_firstDifficulty;
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private Sprite level_background_accessable;
    [SerializeField] private Sprite level_background_not_accessable;

    async void Start()
    {
        //initialize UI fields - set colors of first toggles
        tgl_firstMap.isOn = false; 
        tgl_firstMap.isOn = true;
        tgl_firstDifficulty.isOn = false;
        tgl_firstDifficulty.isOn = true;
        GameObject myButtonObject; 
        Button myButton;


        Debug.Log("Count vorher: " + playerDataSO.EasyLevelPassed.Count);
        Debug.Log("LOAD Level information from database");
        playerDataSO.EasyLevelPassed = new List<LevelPassed>();
        await FirebaseManagerGame.instance.LoadLevelPassedAsync();

        Debug.Log("Count nachher: " + playerDataSO.EasyLevelPassed.Count);
        Debug.Log("Level enable / disable START");
        foreach (LevelPassed lvlPassed in playerDataSO.EasyLevelPassed)
        {
            lvlPassed.DebugOut();
            if (lvlPassed.SubLevel.Count == 0)
            {
                Debug.Log("lvlPassed.SubLevel.Count == 0");
                myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-1");
                myButton = myButtonObject.GetComponent<Button>();
                myButton.image.sprite = level_background_accessable;
                myButton.interactable = true; 
            }
            else if (lvlPassed.SubLevel.Count < 5)
            {
                Debug.Log("lvlPassed.SubLevel.Count < 5");
                Debug.Log("lvlPassed.SubLevel.Count: " + lvlPassed.SubLevel.Count);
                for (int i = 1; i <= lvlPassed.SubLevel.Count+1; i++) //+1 in order to enable one level after last one passed
                {
                    Debug.Log("ENABLE lvl name = " + "lvl." + lvlPassed.MainLevel + "-" + i);
                    myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + i);
                    myButton = myButtonObject.GetComponent<Button>();
                    myButton.image.sprite = level_background_accessable;
                    myButton.interactable = true;
                }
                for (int i = lvlPassed.SubLevel.Count + 2; i < 6; i++) //disable all other level
                {
                    Debug.Log("DISABLE lvl name = " + "lvl." + lvlPassed.MainLevel + "-" + i);
                    myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + i);
                    myButton = myButtonObject.GetComponent<Button>();
                    myButton.image.sprite = level_background_not_accessable;
                    myButton.interactable = false;
                }
            }
            else if (lvlPassed.SubLevel.Count == 5)
            {
                Debug.Log("lvlPassed.SubLevel.Count == 5");
                for (int i = 1; i <= lvlPassed.SubLevel.Count; i++) 
                {
                    myButtonObject = GameObject.Find("lvl." + lvlPassed.MainLevel + "-" + i);
                    myButton = myButtonObject.GetComponent<Button>();
                    myButton.image.sprite = level_background_accessable;
                    myButton.interactable = true;
                }
            }
        }
    }

    void Update()
    {
        
    }
}
