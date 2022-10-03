using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    float timer = 0.0f;
    int timeRemaining = 10;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStart)
        {

            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
            if (seconds > 0)
            {
                timer = 0;
                UpdateTimer();
            }
        }
    }

    private void UpdateTimer() {
        gameObject.GetComponent<TextMeshProUGUI>().SetText($"Next season in: \n {timeRemaining}");
        timeRemaining--;
        if (timeRemaining <= -1) {
            gameManager.ChangeSeason();
            timeRemaining = 10;
        }
    }
}
