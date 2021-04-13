using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PlayerData playerData;

    public float forwardForce;
    public float sidewaysForce;
    private float powerupForwardForce = 0f;
    private bool powerupBigger = false;

    public Transform playerTransform;
    public Rigidbody rigidBody;

    private float screenCenterX;

    private void Awake()
    {
        playerData = PlayerPersistence.GetData();
    }
    private void OnDestroy()
    {
        PlayerPersistence.SaveData(playerData);
    }

    private void Start()
    {
        // save the horizontal center of the screen in order to determine the left and right touch control
        screenCenterX = Screen.width * 0.5f;
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
        //move forward, if applicable with boost
        rigidBody.AddForce(0, 0, (forwardForce + powerupForwardForce) * Time.deltaTime);

        //set powerupForwardForce to 0 again, after it was considered
        if (powerupForwardForce != 0f)
        {
            //Debug.Log("BOOST#############" + boostForwardForce.ToString());
            powerupForwardForce = 0f;
        }

        //left/right movement
        if (Input.GetButton("Right"))
        {
            rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetButton("Left"))
        {
            rigidBody.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
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
                rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }
            // if the touch position is to the left of center
            else if (firstTouch.position.x < screenCenterX)
            {
                rigidBody.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
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
        powerupForwardForce = playerData.BaseForwardForce * percantageOfBaseForwardForce;
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
