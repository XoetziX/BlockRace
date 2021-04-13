using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody rigidBody;

    [SerializeField] private PlayerData _playerData;
    private float _currentForwardForce;
    private float _currentSidewaysForce;

    private float powerupForwardForce = 0f;
    private bool powerupBigger = false;


    private float screenCenterX;

    private void Start()
    {
        // save the horizontal center of the screen in order to determine the left and right touch control
        screenCenterX = Screen.width * 0.5f;
        _currentForwardForce = _playerData.GetCurrentForwardForce();
        _currentSidewaysForce = _playerData.getCurrentSidewayForce();
    }
        // Update is called once per frame
    void Update()
    {
        DoKeyBoardMovement();
        DoTouchMovementTODOSinnvollInEineMovementMethodeIntegrieren();
        CalculatePowerUps();
        CheckGameOver();

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
        Debug.Log("DRIN");
        //move forward, if applicable with boost
        rigidBody.AddForce(0, 0, (_currentForwardForce + powerupForwardForce) * Time.deltaTime);

        //set powerupForwardForce to 0 again, after it was considered
        if (powerupForwardForce != 0f)
        {
            //Debug.Log("BOOST#############" + boostForwardForce.ToString());
            powerupForwardForce = 0f;
        }

        //left/right movement
        if (Input.GetButton("Right"))
        {
            rigidBody.AddForce(_currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetButton("Left"))
        {
            rigidBody.AddForce(-_currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
    }

    private void DoTouchMovementTODOSinnvollInEineMovementMethodeIntegrieren()
    {
        // if there are any touches currently
        if (Input.touchCount > 0)
        {
            // get the first one
            Touch firstTouch = Input.GetTouch(0);


            // if the touch position is to the right of center
            if (firstTouch.position.x > screenCenterX)
            {
                rigidBody.AddForce(_currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }
            // if the touch position is to the left of center
            else if (firstTouch.position.x < screenCenterX)
            {
                rigidBody.AddForce(-_currentSidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

        }
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

    public void setBoostForward(float percantageOfBaseForwardForce)
    {
        powerupForwardForce = _playerData.BaseForwardForce * percantageOfBaseForwardForce;
    }

    /*public void setBoostBigger()
    {
        powerupBigger = true;
    }*/
    public void setBoostBigger() => powerupBigger = true;

    private void MakeMeNormalAgain()
    {
        playerTransform.localScale -= new Vector3(1, 1, 1);
    }
}
