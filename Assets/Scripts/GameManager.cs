using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }
    public GameObject player { get; private set; }

    [SerializeField]
    public GameObject endMenu;    

    [SerializeField]
    private GameObject openingUI;

    [SerializeField]
    private GameObject scoreBoard;

    [SerializeField]
    private int seed;

    [SerializeField]
    private float pauseLength;

    [SerializeField]
    private int maxLevel;

    [SerializeField]
    private float[] spawnDelays;

    [SerializeField]
    private int[] killsRequired;

    [SerializeField]
    private Transform[] spawnPoints;    

    private GameObject[] enemies = new GameObject[3];

    public static bool gamePause;
    public static int score;
    public static int currentLevel;
    public static int killCount;
  
    private float currDelay;
    private float spawnTimer;
    private float pauseTimer;
    private int currentStep;
    private int spawnCount;

    void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        enemies[0] = Resources.Load<GameObject>("Prefabs/" + "ZomBear");
        enemies[1] = Resources.Load<GameObject>("Prefabs/" + "Zombunny");
        enemies[2] = Resources.Load<GameObject>("Prefabs/" + "Hellephant");
        endMenu.SetActive(false);
        currentLevel = 1;
        currentStep = 1;
        currDelay = spawnDelays[0];
        score = 0;       
        gamePause = true;
        scoreBoard.SetActive(false);
        spawnTimer = 0;
        pauseTimer = pauseLength;
        spawnCount = 0;
    }

    void Start() {
        openingUI.SetActive(false);
        StartGame();
    }

    void Update() {

        if (spawnTimer > 0) spawnTimer -= Time.deltaTime;
        if (pauseTimer > 0) pauseTimer -= Time.deltaTime;


        if (gamePause && pauseTimer <= 0)
            gamePause = false;
        
        if (!gamePause && spawnTimer <= 0)
            WaveManager();        

        //Level Up
        if (killCount >= killsRequired[currentLevel - 1]) {
            if (currentLevel < maxLevel) currentLevel++;
            Debug.Log("Current Level:" + currentLevel);
            currDelay = spawnDelays[currentLevel - 1];
            killCount = 0;
            spawnCount = 0;
            currentStep = 1;
            //Pause before next 
            gamePause = true;
            pauseTimer = pauseLength;
        } 
        
        if(endMenu.activeSelf) {
            scoreBoard.SetActive(false);
        }
    }

    public void PauseGame() {        
        gamePause = true;
    }

    public void EndGame() {
        if(!endMenu.activeSelf) { //Makes sure not called multiple times(?)
            Debug.Log("EndGame");
            endMenu.SetActive(true);
            scoreBoard.SetActive(false);
            LeaderboardScript.updatedScore = false;
            //LeaderboardScript.instance.UpdateScores();
        }        
    }

    public void ResetGame() {
        Debug.Log("ResetGame");
        endMenu.SetActive(false);
        currentLevel = 1;        
        currentStep = 1;
        currDelay = spawnDelays[0];
        score = 0;
        spawnTimer = 0;
        spawnCount = 0;
        pauseTimer = pauseLength;
        StartGame();
    }

    public void StartGame() {
        scoreBoard.SetActive(true);
        Debug.Log("StartGame");
        Random.InitState(seed);
        gamePause = false;        
    }

    void WaveManager() {

        int bear = 1;
        int bunny = 2;
        int elephant = 3;

        if (spawnCount >= killsRequired[currentLevel - 1])
            currentStep++;

        //Each level manually designed. This could've been done more efficiently...
        switch (currentLevel) {
            case 1: //Tutorial Wave
                switch (currentStep) {
                    case 1: 
                        SpawnEnemy(bear, 11); //Spawn bear far away
                        spawnTimer = currDelay;
                        currentStep++;
                        break;
                    case 2:
                        SpawnEnemy(bear, 9); //Quick Spawn Bears
                        spawnTimer = currDelay / 3.0f;
                        currentStep++;
                        break;
                    case 3:
                        SpawnEnemy(bear, 15);
                        spawnTimer = currDelay / 3.0f;
                        currentStep++;
                        break;
                    case 4:
                        SpawnEnemy(bear, 4);
                        spawnTimer = currDelay / 3.0f;
                        currentStep++;
                        break;            
                    default:
                        break;
                }
                break;
            case 2: //Beginner Round                
                switch (currentStep) {
                    case 1:
                        SpawnEnemy(bear, 0); //Spawn random bear
                        spawnTimer = currDelay;
                        break;            
                    default:
                        break;
                }
                break;
            case 3: //Bunny Invasion
                switch (currentStep) {
                    case 1:
                        SpawnEnemy(bunny, 11); //Spawn 3 Bunnies far away
                        SpawnEnemy(bunny, 9); //Spawn 3 Bunnies far away
                        SpawnEnemy(bunny, 12); //Spawn 3 Bunnies far away
                        spawnTimer = currDelay;
                        currentStep++;
                        break;
                    case 2:
                        SpawnEnemy(bear, 0); //Quick Spawn Bears and Bunnies
                        SpawnEnemy(bunny, 0);
                        spawnTimer = currDelay / 2.0f;
                        break;
                    default:
                        break;
                }
                break;
            case 4: //Intermediate Wave
                switch (currentStep) {
                    case 1:
                        SpawnEnemy(Random.Range(1, 3), 0); //Spawn random bears or bunnies in bursts of 2
                        SpawnEnemy(Random.Range(1, 3), 0); 
                        spawnTimer = currDelay;
                        break;
                    default:
                        break;
                }
                break;
            case 5: //Elephants Storm
                switch (currentStep) {
                    case 1:
                        SpawnEnemy(elephant, 2); //Spawn 1 Elephant
                        SpawnEnemy(elephant, 8); //Spawn 1 Elephant
                        spawnTimer = currDelay / 3.0f;
                        currentStep++;
                        break;
                    case 2:
                        SpawnEnemy(bunny, 1); //Spawn 3 Bunnies to compare speed and health
                        SpawnEnemy(bear, 5); 
                        SpawnEnemy(bunny, 3);
                        spawnTimer = currDelay;
                        currentStep++;
                        break;
                    case 3:
                        SpawnEnemy(0, 0); //Spawn Random Enemy and Elephant
                        SpawnEnemy(elephant, 0); 
                        spawnTimer = currDelay;
                        break;
                    default:
                        break;
                }
                break;
            case 6: //Advanced Round
                switch (currentStep) {
                    case 1:
                        SpawnEnemy(0, 0); //Spawn random bears or bunnies in bursts
                        SpawnEnemy(Random.Range(1, 3), 0); 
                        SpawnEnemy(Random.Range(1, 3), 0);
                        SpawnEnemy(elephant, 0);
                        spawnTimer = currDelay;
                        break;
                    default:
                        break;
                }
                break;
            case 7: //Impossible Round
                switch (currentStep) {
                    case 1: //Die
                        SpawnEnemy(0, 0); //Continous Random Enemies
                        spawnTimer = currDelay;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }        
    }

    //Spawns a single enemy with given parameters or at random
    void SpawnEnemy(int enemyType = 0, int spawnPoint = 0) {

        int i;

        //Chooose Spawnpoint
        if (spawnPoint > 0 && spawnPoint <= spawnPoints.Length)
            i = spawnPoint - 1;
        else
            i = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPos = spawnPoints[i].position;

        //Choose Enemy Type
        if (enemyType > 0 && enemyType <= enemies.Length)
            i = enemyType - 1;
        else
            i = Random.Range(0, enemies.Length);

        //Spawn
        Instantiate(enemies[i], spawnPos, Quaternion.identity);
        spawnCount++;
    }

}
