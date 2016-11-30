﻿using UnityEngine;
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

    private int[] topScores = new int[10];
    private string[] topNames = new string[10];

    void Awake() {
        instance = this;
        currentPlayer = "ZZZ";
        updatedScore = false;
    }

	// Use this for initialization
	void Start () {                
        loadLeaderboard();
	}

    void loadLeaderboard() {
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
            
    }    

    void Update() {
        playerText.text = currentPlayer;
    }
	
	public void UpdateScores () {

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
                    }
                }
                else {
                    topScores[i + 1] = GameManager.score;
                    topNames[i + 1] = currentPlayer;
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
