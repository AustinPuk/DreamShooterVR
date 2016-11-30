using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveText : MonoBehaviour {

    private Text waveText;

    void Start() {
        waveText = GetComponent<Text>();
    }

    void Update() {
        waveText.text = GameManager.currentLevel.ToString() + " / 7";
    }
}
