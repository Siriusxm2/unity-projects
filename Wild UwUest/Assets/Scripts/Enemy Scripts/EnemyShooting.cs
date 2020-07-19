using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private Transform player;
    public static bool shooting = false;
    private SphereCollider coll;

    private void Awake() {
        coll = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            float dist = Vector3.Distance(player.transform.position, this.transform.position);
            transform.LookAt(other.gameObject.transform.position);
            if (dist < maxDistance)
                shooting = true;
            else
                shooting = false;
        }
    }
}
