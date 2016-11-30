using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private float baseSpeed = 2.0f;

    [SerializeField]
    private float stopDist = 0.9f;

    [SerializeField]
    private float baseHealth = 30.0f;

    [SerializeField]
    private ParticleSystem blood;

    private Animator myAnimator;

    private bool isSinking = false;
    private float sinkSpeed = 2.5f;

    private float hitRate = 3.0f;
    private float hitCooldown;

    private float maxHealth;
    private float health;
    private float speed;

    private bool isDead;

    void Start() {        
        myAnimator = GetComponent<Animator>();
        hitCooldown = 0.0f;
        maxHealth = baseHealth; //Maybe add a better formula for increasing health later
        health = maxHealth;
        speed = baseSpeed + baseSpeed * GameManager.currentLevel * 0.1f;
        isDead = false;
    }
	
	void Update () {

        if (hitCooldown > 0.0f) hitCooldown -= Time.deltaTime;

        if (isSinking) {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);            
        }

        if(!isDead) {

            if (GameManager.instance.player.GetComponent<PlayerController>().health < 0.0f) {
                isDead = true;
                myAnimator.SetTrigger("Dies");
                return;
            }

            transform.LookAt(GameManager.instance.player.transform);

            if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) > stopDist) {
                transform.position += transform.forward * speed * Time.deltaTime;
                //Debug.Log("Moving Enemy");
            }
            else {
                //Attacking Player
                if (hitCooldown <= 0.0f) {
                    Debug.Log("Attacking Player");
                    GameManager.instance.player.GetComponent<PlayerController>().health -= 10.0f;
                    StartCoroutine(GameManager.instance.player.GetComponent<PlayerController>().flashDamage());
                    hitCooldown = hitRate;
                }
            }
        }        
    }

    public void takeDamage() {
        if (!isDead) {
            health -= 10.0f;
            blood.Stop();
            blood.Play();
            if (health <= 0.0f) {
                GameManager.score += 500;
                GameManager.killCount++;
                isDead = true;
                myAnimator.SetTrigger("Dies");
            }
        }        
    }

    public void StartSinking() {        
        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;

        isSinking = true;

        // After 2 seconds destory the enemy.
        Destroy(gameObject, 2f);
    }
}
