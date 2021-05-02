using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement player;
    bool collidedRelevantly = false;

    private void OnCollisionEnter(Collision collision)
    {

        //Vermeiden von mehrfach Collider Events beim Treffen von mehreren Hindernissen auf einmal.
        if (!collidedRelevantly)
        {
            if (collision.collider.tag == "Obstacle")
            {
                collidedRelevantly = true;
                Invoke("StopPlayerMovement", 0f);
                FindObjectOfType<GameManager>().GameOver();
            }
            else if (collision.collider.tag == "Goal")
            {
                collidedRelevantly = true;
                FindObjectOfType<GameManager>().CompleteLevel();
                Invoke("StopPlayerMovement", 0f);
            }
            else if (collision.collider.tag == "SpeedUp")
            {
                collision.gameObject.SetActive(false);
                player.SetBoostForward(100f);
            }
            else if (collision.collider.tag == "Bigger")
            {
                collision.gameObject.SetActive(false);
                player.SetBoostBigger();
            }
        }

    }

    private void StopPlayerMovement()
    {
        player.enabled = false;

    }

}
