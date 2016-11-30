using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float maxHealth = 100.0f;

    [SerializeField]
    Light myLight;

    [SerializeField]
    Gun myGun;

    [SerializeField]
    private float shootDelay = 0.1f;

    [SerializeField]
    private float regenRate = 0.05f;

    [SerializeField]
    StartScript startButton;

    private Camera myCamera;
    private Transform myBody;   

    private float shootCooldown;
    private float regenCooldown;

    private bool isDead;

    private bool alreadyStart;

    [HideInInspector]
    public float health;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 0.1F;
    public float sensitivityY = 0.1F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    void Awake() {
        myCamera = transform.FindChild("MainCamera").GetComponent<Camera>();
        myBody = transform.FindChild("ModelRotation");        
    }

    void Start() {
        alreadyStart = false;
        myCamera.GetComponent<VRInput>().OnClick += Shoot;
        myCamera.GetComponent<VRInput>().OnClick += StartGame;
        shootCooldown = 0.0f;
        regenCooldown = 0.0f;
        health = maxHealth;
        isDead = false;
    }

	void Update () {
        //Rotates Body with Camera
        myBody.transform.localRotation = myCamera.transform.localRotation;

        if (shootCooldown > 0.0f) shootCooldown -= Time.deltaTime;
        if (regenCooldown > 0.0f) regenCooldown -= Time.deltaTime;        

        if(!isDead) {
            showDamage();
            healthRegen();
            checkDeath();
        }               

        if (Input.GetMouseButtonDown(0)) {
            Shoot();
            StartGame();
        }

        /*
                
        
        if(Input.GetKey(KeyCode.W)) {
            transform.Rotate(-5.0f, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Rotate(5.0f, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(0.0f, -5.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(0.0f, 5.0f, 0.0f, Space.World);
        }

        Cursor.visible = false;
        
        if (axes == RotationAxes.MouseXAndY) {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }        
        */
    }

    void showDamage() {        
        float colorChange = Mathf.Lerp(0f, 1.0f, (health / maxHealth)); 
        myLight.color = new Color(1.0f, colorChange, colorChange, 1.0f);
    }

    public IEnumerator flashDamage() {
        myLight.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        myLight.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        myLight.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        myLight.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        myLight.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        if(isDead)
            myLight.color = new Color(0.3f, 0.0f, 0.0f, 1.0f);
    }
    
    void checkDeath() {
        if(health < 0.0f) {
            isDead = true;
            myLight.color = new Color(0.3f, 0.0f, 0.0f, 1.0f);
            StartCoroutine(deathFade());
            GameManager.instance.PauseGame();
        }
    }

    void healthRegen() {
        if(health < maxHealth && regenCooldown <= 0.0f) {
            health += 0.1f;
            regenCooldown = regenRate;
        }        
    }

    private void Shoot() {

        if (!alreadyStart)
            return;

        if (shootCooldown > 0.0f) {
            return;
        }            
        shootCooldown = shootDelay;

        myGun.gunShot();
    }

    private void StartGame() {
        if (!alreadyStart) {
            alreadyStart = true;
            startButton.OnClick();
        }
    }

    IEnumerator deathFade() {
        yield return new WaitForSeconds(3.0f);
        resetPlayer();
    } 
    
    void resetPlayer() {
        shootCooldown = 0.0f;
        regenCooldown = 0.0f;
        health = maxHealth;
        isDead = false;
        GameManager.instance.EndGame();        
    }
}
