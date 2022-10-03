using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    GameManager gameManager;
    public Pick pick;
    public Water water;
    public Seed seed;
    public GameObject soilArrow;
    public Shop shop;
    public GameObject dropArrow;

    public Canvas tutorialCanvas;
    Player player;
    public int seq = 0;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        seed.Init(2);
    }

    private void Update()
    {
    }

    public void NextSequence() {
        if (seq == 0)
        {
            seed.ToggleArrow(false);
            soilArrow.SetActive(true);
        }
        else if (seq == 1)
        {
            soilArrow.SetActive(false);
            water.ToggleArrow(true);
        }
        else if (seq == 2)
        {
            seed.ToggleArrow(true);
            water.ToggleArrow(false);
        }
        else if (seq == 3)
        {
            gameManager.ChangeSeason();

        }
        else if (seq == 4)
        {
            gameManager.ChangeSeason();
            dropArrow.SetActive(true);
            seed.ToggleArrow(false);
        }
        else if (seq == 5)
        {
            seed.ToggleArrow(true);
            dropArrow.SetActive(false);
        }
        else if (seq == 6) { 
            shop.ToggleArrow(true);
            seed.ToggleArrow(false);
        }
        else if (seq == 7)
        {
            shop.ToggleArrow(false);
            pick.ToggleArrow(true);
        }
        else if (seq == 8)
        {
            soilArrow.SetActive(true);
            pick.ToggleArrow(false);
        }
        else if (seq == 9)
        {
            pick.ToggleArrow(false);
            water.ToggleArrow(false);
            soilArrow.SetActive(false);
            tutorialCanvas.gameObject.SetActive(false);
            gameManager.ReadyGame();
        }
        seq++;
    }
}
