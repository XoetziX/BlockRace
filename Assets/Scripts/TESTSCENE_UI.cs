using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTSCENE_UI : MonoBehaviour
{
    [SerializeField] private ToggleGroup tg_1;
    [SerializeField] private ToggleGroup tg_2;
    [SerializeField] private Toggle tgl_1;
    [SerializeField] private Toggle tgl_2;
    public Color toggleOnColor;
    public Color toggleOffColor;

    [SerializeField] private Sprite level_background_accessable;
    [SerializeField] private Sprite level_background_not_accessable;
    [SerializeField] private Button level_button;

    // Start is called before the first frame update
    void Start()
    {
        //var toggles1 = tg_1.GetComponentsInChildren<Toggle>();
        //var toggles2 = tg_2.GetComponentsInChildren<Toggle>();
        ////Debug.Log("TOGGLE - playerData: " + playerData.ChoosenDifficulty + " Difficulty: " + Difficulty.easy + " true? " + (playerData.ChoosenDifficulty == Difficulty.easy));
        //toggles1[0].isOn = true;
        //toggles2[1].isOn = true;

        //if (playerData.ChoosenDifficulty == Difficulty.easy)
        //    toggles[0].isOn = true;
        //else if (playerData.ChoosenDifficulty == Difficulty.medium)
        //    toggles[1].isOn = true;
        //else
        //    toggles[2].isOn = true;
    }
    public void DoItBaby1()
    {
        //tgl_1.isOn = true;
        level_button.image.sprite = level_background_accessable;
    }
    public void DoItBaby2()
    {
        level_button.image.sprite = level_background_not_accessable;
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
