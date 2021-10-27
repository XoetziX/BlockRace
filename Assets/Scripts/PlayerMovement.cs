using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private PlayerDataSO playerData;
    //[SerializeField] private PlayerDataVar _playerDataVar;
    [SerializeField] private GameSettingsSO gameSettings;

    private float baseForwardForce;
    private float currentForwardForce; //can be improved with skills (later on)
    private float currentSidewaysForce;

    private float powerupForwardForce = 0f;
    private bool powerupBigger = false;


    private float screenCenterX;

    private void Start()
    {
        // save the horizontal center of the screen in order to determine the left and right touch control
        screenCenterX = Screen.width * 0.5f;
        baseForwardForce = (int)playerData.ChoosenDifficulty;
        currentForwardForce = GetCurrentForwardForce();
        currentSidewaysForce = GetCurrentSidewayForce();
    }
    private float GetCurrentForwardForce()
    {
        return (int)playerData.ChoosenDifficulty;
    }
    public float GetCurrentSidewayForce()
    {
        return playerData.BaseSidewayForce;
    }

    // Update is called once per frame
    void Update()
    {
        DoForwardMovement();
        DoKeyBoardMovement();
        DoTouchMovement();
        CalculatePowerUps();
        CheckGameOver();

    }

    private void DoForwardMovement()
    {
        //move forward, if applicable with boost
        rigidBody.AddForce(0, 0, (currentForwardForce + powerupForwardForce) * Time.deltaTime);
        //set powerupForwardForce to 0 again, after it was considered
        if (powerupForwardForce != 0f)
        {
            //Debug.Log("BOOST#############" + boostForwardForce.ToString());
            powerupForwardForce = 0f;
        }
    }

    private void CheckGameOver()
    {
        //fall of the track? -> Game Over
        if (rigidBody.position.y < 0.9f)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.GameOver();
        }
    }

    private void DoKeyBoardMovement()
    {
        //left/right movement
        if (Input.GetButton("Right"))
        {
            rigidBody.AddForce(currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            CalculateSteeringHandicap();
        }
        if (Input.GetButton("Left"))
        {
            rigidBody.AddForce(-currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            CalculateSteeringHandicap();
        }
    }

    private void DoTouchMovement()
    {
        // if there are any touches currently
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x > screenCenterX) // if the touch position is to the right of center
            {
                rigidBody.AddForce(currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                CalculateSteeringHandicap();
            }            
            else if (touch.position.x < screenCenterX) // if the touch position is to the left of center
            {
                rigidBody.AddForce(-currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                CalculateSteeringHandicap();
            }
        }
    }

    private void CalculateSteeringHandicap()
    {
        powerupForwardForce = baseForwardForce * -0.2f;
    }

    private void CalculatePowerUps()
    {
        if (powerupBigger)
        {
            playerTransform.position += new Vector3(0, 0.5f, 0);
            playerTransform.localScale += new Vector3(1, 1, 1);
            Invoke("MakeMeNormalAgain", 2f);
            powerupBigger = false;
        }
    }

    public void SetBoostForward(float percantageOfBaseForwardForce)
    {
        powerupForwardForce = baseForwardForce * percantageOfBaseForwardForce;
    }

    /*public void setBoostBigger()
    {
        powerupBigger = true;
    }*/
    public void SetBoostBigger() => powerupBigger = true;

    private void MakeMeNormalAgain()
    {
        playerTransform.localScale -= new Vector3(1, 1, 1);
    }
}
