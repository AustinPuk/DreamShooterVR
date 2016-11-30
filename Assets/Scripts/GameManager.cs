using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }
    public GameObject player { get; private set; }

    [SerializeField]
    public GameObject endMenu;

    [SerializeField]
    private int maxLevel; 

    [SerializeField]
    private float[] spawnDelays;

    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private int[] killsRequired;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameObject openingUI;

    private GameObject[] enemies = new GameObject[2];

    public static bool gamePause;
    public static int score;
    public static int currentLevel;
    public static int killCount;

    private float currDelay;
    

    void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        enemies[0] = Resources.Load<GameObject>("Prefabs/" + "ZomBear");
        enemies[1] = Resources.Load<GameObject>("Prefabs/" + "Zombunny");
        endMenu.SetActive(false);
        currentLevel = 1;
        currDelay = spawnDelays[0];
        score = 0;
    }

    void Start() {
        openingUI.SetActive(false);
        StartGame();
    }

    void Update() {        
        if (killCount >= killsRequired[currentLevel - 1]) {            
            if (currentLevel < maxLevel) currentLevel++;
            Debug.Log("Level:" + currentLevel);
            currDelay = spawnDelays[currentLevel - 1];
            killCount = 0;
        }
        scoreText.text = score.ToString("00000000");
    }

    IEnumerator SpawnEnemies() {
        while (true) {            
            int i = Random.Range(0, spawnPoints.Length);           
            Vector3 spawnPos = spawnPoints[i].position;
            i = Random.Range(0, enemies.Length);
            if(!gamePause) {
                Instantiate(enemies[i], spawnPos, Quaternion.identity);
            }                
            yield return new WaitForSeconds(currDelay);               
        }
    }

    public void PauseGame() {
        StopCoroutine(SpawnEnemies());
        gamePause = true;
    }

    public void ResetGame() {
        Debug.Log("ResetGame");
        endMenu.SetActive(false);
        currentLevel = 1;
        currDelay = spawnDelays[0];
        score = 0;
        StartGame();
    }

    public void StartGame() {
        Debug.Log("StartGame");
        StartCoroutine(SpawnEnemies());
        gamePause = false;
    }

}
