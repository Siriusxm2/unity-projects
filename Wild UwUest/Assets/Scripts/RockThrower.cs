using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower : MonoBehaviour
{
    [SerializeField] private float throwForce = 15f;

    [SerializeField] private GameObject rockPrefab;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ThrowRock();
    }

    private void ThrowRock()
    {
        GameObject rock = Instantiate(rockPrefab, transform.position, transform.rotation);
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        
    }
}
