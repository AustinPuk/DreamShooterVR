using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    [SerializeField]
    private LineRenderer gunLine;
    [SerializeField]
    private Light gunLight;
    [SerializeField]
    private ParticleSystem gunFlare;

    [SerializeField]
    private float maxDistance = 30.0f;

    [SerializeField]
    private float effectDelay;

    private float timer;

    void Update() {
        if (timer > 0.0f) timer -= Time.deltaTime;
        else {
            disableEffects();
        }
    }

    private void enableEffects() {
        gunLine.enabled = true;
        gunFlare.Stop();
        gunFlare.Play();
        gunLight.enabled = true;
    }

    private void disableEffects() {
        gunLine.enabled = false;        
        gunLight.enabled = false;
    }

    public void gunShot() {

        timer = effectDelay;

        enableEffects();

        gunLine.SetPosition(0, transform.position);

        Ray shot = new Ray();
        RaycastHit rayInfo;
        shot.origin = transform.position;
        shot.direction = transform.forward;

        if (Physics.Raycast (shot, out rayInfo, maxDistance)) {
            EnemyController target = rayInfo.collider.GetComponent<EnemyController>();
            if (target != null) {
                target.takeDamage();
            }
            else if (rayInfo.collider.tag == "Button") {
                ResetButton button1 = rayInfo.collider.GetComponent<ResetButton>();
                if(button1 != null) {
                    button1.OnClick();
                }
                else {
                    KeyboardButton key = rayInfo.collider.GetComponent<KeyboardButton>();
                    if (key != null) {
                        key.OnClick();
                    } 
                }
            }            

            gunLine.SetPosition(1, rayInfo.point);
        }
        else {
            gunLine.SetPosition(1, shot.origin + shot.direction * maxDistance);
        }
                
    }
}
