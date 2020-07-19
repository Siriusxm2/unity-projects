using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public static bool alive;

    private void Start() {
        alive = true;
    }

    public void takeDMG(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            alive = false;
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
