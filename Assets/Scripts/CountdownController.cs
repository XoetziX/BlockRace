using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownText;
    public GameSettings gameSettings;

    public void StartLevelCountdown()
    {
        StartCoroutine("CountdownToStart");
    }

    IEnumerator CountdownToStart()
    {
        countdownText.gameObject.SetActive(true);
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSecondsRealtime(0.5f);
            countdownTime--;
        }
        countdownText.text = "GO!";
        gameSettings.ResumeGame = true;
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.gameObject.SetActive(false);
    }
}
