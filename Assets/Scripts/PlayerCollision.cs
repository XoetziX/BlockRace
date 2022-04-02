using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement playerMovement;
    bool collidedRelevantly = false;
    [SerializeField] private GameSettingsSO gameSettings;

    private void OnCollisionEnter(Collision collision)
    {
        //Vermeiden von mehrfach Collider Events beim Treffen von mehreren Hindernissen auf einmal.
        if (!collidedRelevantly)
        {
            if (collision.collider.tag == "Obstacle")
            {
                collidedRelevantly = true;
                Debug.LogWarning("collidedRelevantly = true; never set back to false. May cause problems when more than one collision will be relevant");
                Invoke("StopPlayerMovement", 0f);
                FindObjectOfType<GameManager>().GameOver();
            }
            else if (collision.collider.tag == "Goal")
            {
                collidedRelevantly = true;
                FindObjectOfType<GameManager>().CompleteLevel();
                gameSettings.MoveCamera = false;
                Invoke("StopPlayerMovement", 0f);
            }
            else if (collision.collider.tag == "SpeedUp")
            {
                collision.gameObject.SetActive(false);
                //TODO: Boost value -> new boost SO
                playerMovement.SetBoostForward(100f);
            }
            else if (collision.collider.tag == "Bigger")
            {
                collision.gameObject.SetActive(false);
                playerMovement.SetBoostBigger();
            }
        }

    }

    private void StopPlayerMovement()
    {
        Debug.Log("STOP PLAYER MOVEMENT");
        playerMovement.enabled = false;

    }

}
