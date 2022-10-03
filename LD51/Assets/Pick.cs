using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("pick collided");
            player.inputState = "pick";
            player.itemToPick = gameObject;
        }
    }

    public void ToggleArrow(bool show)
    {
        transform.GetChild(0).gameObject.SetActive(show);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("pick collided");
            player.inputState = "pick";
            player.itemToPick = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("pick exited");
            player.inputState = null;
            player.itemToPick = null;
        }
    }
}
