using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    [SerializeField] private GameSettingsSO gameSettings;

    private void Start()
    {
        gameSettings.MoveCamera = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSettings.MoveCamera)
        {
            transform.position = player.position + offset;
        }
        else
        {
            //Debug.Log("TODO Smooth Camera Stop");
        }
    }
}
