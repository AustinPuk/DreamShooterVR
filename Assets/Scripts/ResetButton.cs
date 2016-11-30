using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

    public void OnClick() {
        GameManager.instance.ResetGame();
    }

}
