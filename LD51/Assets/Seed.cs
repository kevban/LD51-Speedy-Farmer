using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public string seedState = "seed"; //seed, planted, growing, fruit, dead
    public int fruitType = 0; //0 = turnip, 1 = watermelon, 2 = pumpkin
    public Soil soil;
    List<Sprite> sprites = new List<Sprite>();
    GameManager gameManager;
    public SpriteRenderer spriteRenderer;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player" && seedState != "planted") &&
            (collision.tag == "Player" && seedState != "growing")) {
            player.inputState = "pick";
            player.itemToPick = gameObject;
            print("seed collided");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("seed exited");
            player.inputState = null;
            player.itemToPick = null;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.tag == "Player" && seedState != "planted") &&
            (collision.tag == "Player" && seedState != "growing"))
        {
            player.inputState = "pick";
            player.itemToPick = gameObject;
            print("seed collided");
        }
    }

    public void Plant() {
        seedState = "planted";
        spriteRenderer.sprite = sprites[1];
        soil.planted = true;
    }

    public void Init(int type) {
        fruitType = type;
        Sprite[] spriteLib = Resources.LoadAll<Sprite>("tiles2");
        if (type == 0)
        {
            sprites.Add(spriteLib[7]);
            sprites.Add(spriteLib[3]);
            sprites.Add(spriteLib[4]);
            sprites.Add(spriteLib[5]);
            sprites.Add(spriteLib[6]);
        }
        else if (type == 1)
        {
            sprites.Add(spriteLib[10]);
            sprites.Add(spriteLib[3]);
            sprites.Add(spriteLib[4]);
            sprites.Add(spriteLib[8]);
            sprites.Add(spriteLib[9]);
        }
        else if (type == 2) {
            sprites.Add(spriteLib[13]);
            sprites.Add(spriteLib[3]);
            sprites.Add(spriteLib[4]);
            sprites.Add(spriteLib[11]);
            sprites.Add(spriteLib[12]);
        }
        spriteRenderer.sprite = sprites[0];
    }

    public void ToggleArrow(bool show) {
        transform.GetChild(0).gameObject.SetActive(show);
    }

    public void Grow() {
        switch (seedState) {
            case "planted":
                if (soil.watered) {

                    soil.watered = false;
                    spriteRenderer.sprite = sprites[2];
                    seedState = "growing";
                }
                break;
            case "growing":
                print(soil.watered);
                print(fruitType + " " + gameManager.curSeason);
                if (soil.watered)
                {
                    soil.watered = false;
                    if (gameManager.curSeason == fruitType) {
                        spriteRenderer.sprite = sprites[3];
                        seedState = "fruit";
                    }
                }
                break;
            case "fruit":
                spriteRenderer.sprite = sprites[4];
                seedState = "dead";
                break;
            default:
                break;
        }
    }
}
