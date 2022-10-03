using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStartCountDown : MonoBehaviour
{
    float timer = 0;
    bool start = false;
    int timeRemaining = 2;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
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

    public void StartTimer() { 
        start = true;
    }

    private void UpdateTimer()
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText($"Get Ready: \n {timeRemaining}");
        timeRemaining--;
        if (timeRemaining == -1)
        {
            gameObject.GetComponent<TextMeshProUGUI>().SetText($"Go!");
        }
        else if (timeRemaining < -1) {
            start = false;
            gameManager.StartGame();
        }
    }
}
