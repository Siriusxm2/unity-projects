using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public void takeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            Dies();
        }
    }

    private void Dies() {
        Destroy(gameObject);
    }
}
