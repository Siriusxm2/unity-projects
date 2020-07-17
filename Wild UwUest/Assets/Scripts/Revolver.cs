using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float rateOfFire = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform instLocation;
    private float nextFire;

    void Start()
    {
        nextFire = Time.deltaTime + rateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.enemyShooting == true)
        {
            fireRevolver();
        }
    }

    private void fireRevolver()
    {
        if (Time.time > nextFire)
        {
            GameObject bullet = Instantiate(bulletPrefab, instLocation.position, transform.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);

            nextFire = Time.time + rateOfFire;

        }
    }
}
