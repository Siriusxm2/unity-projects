using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower : MonoBehaviour
{
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float timeBetweenShots = 1f;
    [SerializeField] private int bulletsPerTap;

    bool shooting, readyToShoot;
    [SerializeField] private GameObject rockPrefab;

    private void Start() {
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        myInput();
    }

    private void ThrowRock()
    {
        readyToShoot = false;
        GameObject rock = Instantiate(rockPrefab, transform.position, transform.rotation);
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        Invoke("resetThrow", timeBetweenShots);
    }

    private void resetThrow() {
        readyToShoot = true;
    }

    private void myInput() {
        shooting = Input.GetMouseButtonDown(0);

        if (shooting && readyToShoot)
            ThrowRock();
    }
}
