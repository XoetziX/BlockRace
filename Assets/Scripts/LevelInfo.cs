using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private string _levelName;
    public string LevelName { 
        get 
        {
            //TODO add a cool way to extract the relevant infos from the scene/level name (e. g. replace "." with " ")
            _levelName = SceneManager.GetActiveScene().name;
            return _levelName; 
        } 
    }

}
