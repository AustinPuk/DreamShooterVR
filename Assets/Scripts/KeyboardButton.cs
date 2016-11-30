using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class KeyboardButton : MonoBehaviour {

    [SerializeField]
    string key;

    [SerializeField]
    bool isReturn;

    [SerializeField]
    bool isBack;

    public void OnClick() {

        if(isReturn) {
            //Can Only do once
            LeaderboardScript.instance.UpdateScores();
        }
        else if (isBack) {
            if (LeaderboardScript.currentPlayer.Length > 0) {
                LeaderboardScript.currentPlayer = LeaderboardScript.currentPlayer.Substring(0, LeaderboardScript.currentPlayer.Length - 1);
            }
        }
        else if (LeaderboardScript.currentPlayer.Length < 3) {
            LeaderboardScript.currentPlayer += key;
        }            
    }
}
