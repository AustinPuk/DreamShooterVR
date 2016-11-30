using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour {

    private Text scoreText;

    void Start () {
        scoreText = GetComponent<Text>();
    }

    void Update () {
        scoreText.text = GameManager.score.ToString("00000000");
    }
}
