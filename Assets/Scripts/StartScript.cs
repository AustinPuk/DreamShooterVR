using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

    [SerializeField]
    GameObject manager;
    [SerializeField]
    GameObject openingUI;
    [SerializeField]
    GameObject playerModel;
    [SerializeField]
    Transform myCamera;

    [SerializeField]
    private float speed = 0.01f;

    private bool startMove;

    private Vector3 direction;
    private Vector3 wantThis;

    void Start() {
        wantThis = new Vector3(0.0f, 1.47f, 0.0f);
        direction = wantThis - myCamera.position;
        startMove = false;
    }

    void Update() {
        if (startMove) {            
            myCamera.position = myCamera.position + direction * speed * Time.deltaTime;
            if(myCamera.position.z >= 0.0f) {
                myCamera.position = wantThis;
                
                manager.SetActive(true);
                playerModel.SetActive(true);
            }
        }

    }

    public void OnClick() {
        startMove = true;
    }
}
