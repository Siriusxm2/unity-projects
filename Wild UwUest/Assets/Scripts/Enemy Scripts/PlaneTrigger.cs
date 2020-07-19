using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTrigger : MonoBehaviour
{
    Enemy en;

    private void Start() {
        en = GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Rock") {
            if (en != null) {
                en.invSpot = other.gameObject.transform.position;
                en.state = Enemy.State.INVESTIGATE;
            }
        }
    }
}
