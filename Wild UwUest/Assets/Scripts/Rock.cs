using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float dmgPlayer = 25f;
    [SerializeField] private float timeDur = 2f;
    private float timer;

    void Start()
    {
        timer = timeDur;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy en = collision.gameObject.GetComponent<Enemy>();
            if (en != null)
            {
                en.takeDamage(dmgPlayer);
                Destroy(gameObject);
            }
        }
    }
}
