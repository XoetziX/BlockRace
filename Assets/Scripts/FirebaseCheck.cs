using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseCheck : MonoBehaviour
{
    [Header("Firebase Objects")]
    public GameObject FirebaseManagerAuth;
    public GameObject FirebaseManagerGame;

    async void Awake()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            // all good, enable our gameobjects
            // they need not perform the check and can just Initialize
            // as 'instances' 
            Debug.Log("Firebase CHECK successful - ### TODO ### Check whether this makes sense. It will likley be executed after the awake of the two FirebaseManager GOs");
            FirebaseManagerAuth.SetActive(true);
            FirebaseManagerGame.SetActive(true);
        }
        else
        {
            Debug.Log("Firebase Failed");
        }
    }
}
