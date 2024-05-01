using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeScript : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public float countDownTimer;

    public PauseMenu gameObject;

    void Start()
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f);
        countDownTimer--;
        TimerText.SetText(countDownTimer.ToString());
        if(countDownTimer != 0)
        {
            Start();
        }
        else
        {
            // Restart the game
            gameObject = GameObject.FindGameObjectWithTag("GManager").GetComponent<PauseMenu>();
            gameObject.EndGame();

        }

}

public void Update(){
    if (countDownTimer <= 3f)
    {
        // Apply the color red.
        TimerText.color = Color.red;
        }
}
}