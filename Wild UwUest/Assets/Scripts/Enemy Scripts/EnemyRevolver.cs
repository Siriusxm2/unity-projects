using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyRevolver : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float rateOfFire = 3f;
    [SerializeField] private GameObject bulletPrefab;
    private GameObject instLocation;
    private float nextFire;

    void Start()
    {
        nextFire = Time.deltaTime + rateOfFire;
        instLocation = GameObject.Find("Barrel");
    }


    void Update()
    {
        if (Enemy.shooting == true && PlayerHealth.alive == true)
        {
            fireRevolver();
        }
    }

    private void fireRevolver()
    {
        if (Time.time > nextFire)
        {
            GameObject bullet = Instantiate(bulletPrefab, instLocation.transform.position, instLocation.transform.rotation);
            
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(-instLocation.transform.forward * bulletSpeed, ForceMode.Acceleration);

            nextFire = Time.time + rateOfFire;

        }
    }
}
