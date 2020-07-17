using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    public void takeDMG(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
