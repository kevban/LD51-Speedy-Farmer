using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    GameManager gameManager;

    Vector2 movement;
    public float moveSpeed;
    float curSpeed;
    Rigidbody2D rb;
    public GameObject itemToPick;
    public GameObject itemOnHand;
    public Score score;
    Animator animator;
    TutorialManager tutorialManager;
    public Seed tutorialSeed;
    public AudioManager audioManager;

    public string inputState = null;
    public string playerState = null;
    public string itemOnHandStr = null;
    public bool submit = false;

    //tile map
    public Tilemap tilemap;
    public List<TileBase> soilTiles;


    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        curSpeed = movement.sqrMagnitude;

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", curSpeed);

        /*
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", curSpeed);*/

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {

            if (playerState == "holding")
            {
                if (itemOnHand.tag == "Seed")
                {
                    
                    Seed seed = itemOnHand.GetComponent<Seed>();
                    if (seed.seedState == "seed")
                    {
                        Vector3Int tileLocation = tilemap.WorldToCell(transform.position);
                        TileBase playerTile = tilemap.GetTile(tileLocation);
                        print(new Vector3Int(tileLocation.x + 5, tileLocation.y + 3));
                        if (playerTile == soilTiles[0] || playerTile == soilTiles[1])
                        {
                            if (gameManager.CheckSoil(tileLocation))
                            {
                                if (tutorialManager.seq == 1)
                                {
                                    tutorialManager.NextSequence();
                                }
                                itemOnHand.transform.SetParent(null, true);
                                itemOnHand.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                                itemOnHand.GetComponent<Renderer>().sortingOrder = 1;
                                Vector3 worldPos = tilemap.CellToWorld(tileLocation);
                                worldPos = new Vector3(worldPos.x + 0.5f, worldPos.y + 0.5f, worldPos.z);
                                itemOnHand.transform.position = worldPos;
                                gameManager.Plant(seed, tileLocation);
                                playerState = null;
                                itemOnHand = null;
                                animator.SetBool("Holding", false);
                            }

                        }
                    }
                    else if (seed.seedState == "fruit" && submit)
                    {
                        if (tutorialManager.seq == 7)
                        {
                            tutorialManager.NextSequence();
                        }
                        audioManager.Play("Submit");
                        Destroy(itemOnHand);
                        playerState = null;
                        itemOnHand = null;
                        score.UpdateScore(100);
                        animator.SetBool("Holding", false);
                    }
                    else if (seed.seedState == "dead" && submit) {
                        
                        Destroy(itemOnHand);
                        playerState = null;
                        itemOnHand = null;
                        animator.SetBool("Holding", false);
                    }
                }
                else if (itemOnHand.tag == "Water")
                {

                    Vector3Int tileLocation = tilemap.WorldToCell(transform.position);
                    TileBase playerTile = tilemap.GetTile(tileLocation);
                    if (playerTile == soilTiles[0] || playerTile == soilTiles[1])
                    {
                        
                        gameManager.WaterSoil(tileLocation);
                        if ((tutorialManager.seq == 3 || tutorialManager.seq == 4) && tutorialSeed.soil.watered == true)
                        {
                            tutorialManager.NextSequence();
                        }
                    }
                    else {
                        if (tutorialManager.seq == 5)
                        {
                            tutorialManager.NextSequence();
                        }
                        itemOnHand.transform.SetParent(null, true);
                        itemOnHand.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                        itemOnHand.GetComponent<Renderer>().sortingOrder = 1;
                        itemOnHand.transform.position = new Vector3(itemOnHand.transform.position.x, itemOnHand.transform.position.y - 0.5f, 0);
                        playerState = null;
                        itemOnHand = null;
                        animator.SetBool("Holding", false);
                    }
                }
                else if (itemOnHand.tag == "Pick")
                {
                    Vector3Int tileLocation = tilemap.WorldToCell(transform.position);
                    TileBase playerTile = tilemap.GetTile(tileLocation);
                    if (playerTile == soilTiles[2])
                    {
                        
                        gameManager.FixSoil(tileLocation);
                        if (tutorialManager.seq == 9)
                        {
                            tutorialManager.NextSequence();
                        }
                    }
                    else {
                        itemOnHand.transform.SetParent(null, true);
                        itemOnHand.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                        itemOnHand.GetComponent<Renderer>().sortingOrder = 1;
                        itemOnHand.transform.position = new Vector3(itemOnHand.transform.position.x, itemOnHand.transform.position.y - 0.5f, 0);
                        playerState = null;
                        itemOnHand = null;
                        animator.SetBool("Holding", false);
                    }
                }
                else {
                    itemOnHand.transform.SetParent(null, true);
                    itemOnHand.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                    itemOnHand.GetComponent<Renderer>().sortingOrder = 1;
                    itemOnHand.transform.position = new Vector3(itemOnHand.transform.position.x, itemOnHand.transform.position.y - 0.5f, 0);
                    playerState = null;
                    itemOnHand = null;
                    animator.SetBool("Holding", false);
                }
            }
            else if (inputState == "pick")
            {
                if (tutorialManager.seq == 0 && itemToPick.tag == "Seed")
                {
                    tutorialManager.NextSequence();
                }
                else if (tutorialManager.seq == 2 && itemToPick.tag == "Water")
                {
                    tutorialManager.NextSequence();
                }
                else if (tutorialManager.seq == 8 && itemToPick.tag == "Pick")
                {
                    tutorialManager.NextSequence();
                }
                itemToPick.transform.SetParent(rb.transform, false);
                itemToPick.transform.localPosition = new Vector3(0, 0.5f, 0);
                itemToPick.transform.localScale = Vector3.one;
                itemToPick.GetComponent<Renderer>().sortingOrder = 3;
                playerState = "holding";
                itemOnHand = itemToPick;
                
                if (itemOnHand.tag == "Seed") { 
                    Seed seed = itemOnHand.GetComponent<Seed>();
                    if (seed.seedState == "fruit" || seed.seedState == "dead") {
                        
                        gameManager.Harvest(seed.soil.pos);
                        if (tutorialManager.seq == 6 )
                        {
                            tutorialManager.NextSequence();
                        }
                    }
                }
                animator.SetBool("Holding", true);
            }
        }


    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
