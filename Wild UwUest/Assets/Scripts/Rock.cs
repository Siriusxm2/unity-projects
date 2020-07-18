using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float dmg = 25f;
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

    private void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.CompareTag("Player")) {
            PlayerHealth ph = coll.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) {
                ph.takeDMG(dmg);
                Destroy(gameObject);
            }
        }

        if (coll.gameObject.CompareTag("Enemy")) {
            EnemyHealth eh = coll.gameObject.GetComponent<EnemyHealth>();
            if (eh != null) {
                eh.takeDamage(dmg);
                Destroy(gameObject);
            }
        }
    }
}
