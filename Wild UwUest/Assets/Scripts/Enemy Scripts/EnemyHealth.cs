using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    cameraScript en;

    private void Start() {
        en = GetComponent<cameraScript>();
    }

    public void takeDamage(float amount) {
        health -= amount;
        en.target = en.player;
        en.state = cameraScript.State.CHASE;
        if (health <= 0f) {
            Dies();
        }
    }

    private void Dies() {
        Destroy(gameObject);
    }
}
