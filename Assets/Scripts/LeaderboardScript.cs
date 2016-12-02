using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour {

    public static LeaderboardScript instance { get; private set; }

    [SerializeField]
    Text myText;

    [SerializeField]
    Text playerText;

    [SerializeField]
    bool resetScores;

    public static string currentPlayer;
    public static bool updatedScore; //Can only update once per game
    public static float keyTimer;

    private int[] topScores = new int[10];
    private string[] topNames = new string[10];

    private int lastUpdate;

    void Awake() {
        instance = this;
        currentPlayer = "";
        updatedScore = false;
        keyTimer = 0;
        lastUpdate = -1;
    }

	// Use this for initialization
	void Start () {                
        loadLeaderboard();
	}

    void loadLeaderboard() {
        topScores = new int[10];
        topNames = new string[10];
        for (int i = 0; i < 10; i++) {
            if(PlayerPrefs.HasKey("Score " + i) && !resetScores) {
                topScores[i] = PlayerPrefs.GetInt("Score " + i);
                topNames[i] = PlayerPrefs.GetString("Player " + i);
            }
            else {
                topScores[i] = 0;
                topNames[i] = "ZZZ";
            }                
        }

        //Updates Text on Leaderboard
        myText.text = "Leaderboard \n";
        for (int i = 0; i < 10; i++) {
            myText.text += topNames[i] + " " + topScores[i].ToString("0000000") + "\n";
        }

    }    

    void Update() {
        playerText.text = currentPlayer;        
        if (keyTimer >= 0) keyTimer -= Time.deltaTime;
    }
	
	public void UpdateScores () {

        //Can only update once per game
        if (updatedScore) {
            if (lastUpdate != -1) {
                topNames[lastUpdate] = currentPlayer;

                //Save Updated Scores to Disk
                PlayerPrefs.DeleteAll();
                for (int i = 0; i < 10; i++) {
                    PlayerPrefs.SetInt("Score " + i, topScores[i]);
                    PlayerPrefs.SetString("Player " + i, topNames[i]);
                }
                PlayerPrefs.Save();

                //Updates Text on Leaderboard
                myText.text = "Leaderboard \n";
                for (int i = 0; i < 10; i++) {
                    myText.text += topNames[i] + " " + topScores[i].ToString("0000000") + "\n";
                }
                return;
            }            
        }            

        updatedScore = true;

        loadLeaderboard();

        //Add player to top scores if high enough
        if (GameManager.score > topScores[9]) {
            for (int i = 8; i >= 0; i--) {
                if (GameManager.score > topScores[i]) {
                    topScores[i + 1] = topScores[i];
                    topNames[i + 1] = topNames[i];
                    if (i == 0) {
                        topScores[0] = GameManager.score;
                        topNames[0] = currentPlayer;
                        lastUpdate = 0;
                    }
                }
                else {
                    topScores[i + 1] = GameManager.score;
                    topNames[i + 1] = currentPlayer;
                    lastUpdate = i + 1;
                    break;
                }
            }
        }
        
        //Save Updated Scores to Disk
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < 10; i++) {
            PlayerPrefs.SetInt("Score " + i, topScores[i]);
            PlayerPrefs.SetString("Player " + i, topNames[i]);
        }
        PlayerPrefs.Save();

        //Updates Text on Leaderboard
        myText.text = "Leaderboard \n"; 
        for (int i = 0; i < 10; i++) {
            myText.text += topNames[i] + " " + topScores[i].ToString("0000000") + "\n";
        }
    }
}
