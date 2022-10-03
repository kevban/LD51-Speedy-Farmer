using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    Player player;
    public GameObject chest;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player") {
            print("player entered");
            chest.GetComponent<Animator>().SetBool("open", true);
            player.submit = true;
        }
        
    }

    public void ToggleArrow(bool show)
    {
        transform.GetChild(0).gameObject.SetActive(show);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("player exited");
            chest.GetComponent<Animator>().SetBool("open", false);
            player.submit = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

}
