using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int curSeason = 0; //0 spring, 1 summer, 2, autumn, 3 winter
    public int curYear = 0;
    public string curSeasonStr = "Spring";
    public Canvas gameStartCanvas;
    public GameStartCountDown gameStartCountDown;
    public Canvas uiCanvas;
    public TextMeshProUGUI seasonText;
    public TextMeshProUGUI goScoreText;
    public Soil[,] land = new Soil[8, 5]{
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                                {new Soil(), new Soil(), new Soil(), new Soil(), new Soil()},
                            };
    public List<TileBase> soilTiles; //1 watered, 2 normal, 3 harvested
    public List<GameObject> spawnArea;
    public GameObject seedPrefab;
    public Tilemap tilemap;
    public bool gameStart = false;
    public TextMeshProUGUI finalYearText;
    public Canvas gameOverCanvas;
    public Score score;
    public AudioManager audioManager;

    public Seed tutorialSeed;

    public List<ParticleSystem> particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartGame() { 
        gameStart = true;
        gameStartCanvas.gameObject.SetActive(false);
        uiCanvas.gameObject.SetActive(true);
        ChangeSeason();

    }

    public void ReadyGame() {
        gameStartCountDown.StartTimer();
        gameStartCanvas.gameObject.SetActive(true);
    }

    public void Plant(Seed seed, Vector3Int pos) {
        Vector3Int translatedPos = new Vector3Int(pos.x + 5, pos.y + 3, 0);
        land[translatedPos.x, translatedPos.y].pos = pos;
        seed.soil = land[translatedPos.x, translatedPos.y];
        seed.Plant();
        land[translatedPos.x, translatedPos.y].seed = seed;
    }

    public void Harvest(Vector3Int pos) {
        Vector3Int translatedPos = new Vector3Int(pos.x + 5, pos.y + 3, 0);
        land[translatedPos.x, translatedPos.y].seed = null;
        land[translatedPos.x, translatedPos.y].pos = pos;
        land[translatedPos.x, translatedPos.y].harvested = true;
        land[translatedPos.x, translatedPos.y].planted = false;
        tilemap.SetTile(pos, soilTiles[2]);
    }

    public bool CheckSoil(Vector3Int pos) {
        Vector3Int translatedPos = new Vector3Int(pos.x + 5, pos.y + 3, 0);
        return !land[translatedPos.x, translatedPos.y].planted;
    }

    public void WaterSoil(Vector3Int pos) {
        Vector3Int translatedPos = new Vector3Int(pos.x + 5, pos.y + 3, 0);
        land[translatedPos.x, translatedPos.y].watered = true;
        land[translatedPos.x, translatedPos.y].pos = pos;
        audioManager.Play("Water");
        tilemap.SetTile(pos, soilTiles[0]);
        
    }

    public void FixSoil(Vector3Int pos) {
        Vector3Int translatedPos = new Vector3Int(pos.x + 5, pos.y + 3, 0);
        land[translatedPos.x, translatedPos.y].watered = true;
        land[translatedPos.x, translatedPos.y].pos = pos;
        audioManager.Play("Pick");
        tilemap.SetTile(pos, soilTiles[1]);
    }

    public void SpawnSeed(int amt) {
        for (int i = 0; i < amt; i++)
        {
            int randSpawn = Random.Range(0, spawnArea.Count);
            int randomCrop = Random.Range(0, 3);
            GameObject seedObj = Instantiate(seedPrefab, new Vector3(spawnArea[randSpawn].transform.position.x + Random.Range(0f, 10f) / 10f,
                spawnArea[randSpawn].transform.position.y + Random.Range(0f, 30f) / 10f, 0), Quaternion.identity);
            Seed seed = seedObj.GetComponent<Seed>();
            seed.Init(randomCrop);
        }
    }

    public void GameOver() { 
        gameOverCanvas.gameObject.SetActive(true);
        goScoreText.text = $"{score.score}";
    }

    public void PlayAgain() {
        SceneManager.LoadScene(1);
    }

    public void ChangeSeason() {
        if (gameStart) { 
            SpawnSeed(2); 
        }
        switch (curSeason) { 
            case 0:
                curSeason = 1;
                curSeasonStr = "Summer";
                seasonText.text = curSeasonStr;
                seasonText.color = Color.green;
                particles[1].Play();
                break;
            case 1:
                curSeason = 2;
                curSeasonStr = "Autumn";
                seasonText.text = curSeasonStr;
                seasonText.color = Color.red;
                particles[2].Play();
                break;
            case 2:
                curSeason = 3;
                curSeasonStr = "Winter";
                seasonText.text = curSeasonStr;
                seasonText.color = Color.blue;
                particles[3].Play();
                break;
            default:
                if (curYear >= 4) {
                    GameOver();
                    break;
                }
                curYear++;
                curSeason = 0;
                curSeasonStr = "Spring";
                seasonText.text = curSeasonStr;
                seasonText.color = Color.yellow;
                particles[0].Play();
                break;
                
        }
        if (curYear >= 4)
        {
            finalYearText.gameObject.SetActive(true);
        }
        if (gameStart) {
            audioManager.SwitchTrack($"{curSeason + 2}");
        }
        
        for (int i = 0; i < land.GetLength(0); i++) {
            for (int k = 0; k < land.GetLength(1); k++) {
                if (land[i, k].seed != null) {
                    land[i, k].seed.Grow();
                    Vector3Int pos = new Vector3Int(i - 5, k - 3, 0);
                    tilemap.SetTile(pos, soilTiles[1]);
                }
            }
        }
    }
}
