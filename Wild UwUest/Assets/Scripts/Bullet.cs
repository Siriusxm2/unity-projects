using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float dmgEnemy = 50f;
    [SerializeField] private float timeDurr = 2f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeDurr;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.takeDMG(dmgEnemy);
                Destroy(gameObject);
            }
        }
    }
}
