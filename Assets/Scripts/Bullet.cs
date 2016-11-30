using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = 10.0f;

    private Vector3 direction;    

    void Start() {
        direction = transform.up;
        Destroy(gameObject, 5.0f);
    }

	void Update() {        
        transform.position += direction * Time.deltaTime * movementSpeed;
    }

    void OnTriggerEnter(Collider other) {

        //Debug.Log("Bullet Collision");

        if(other.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        }

    }
}
