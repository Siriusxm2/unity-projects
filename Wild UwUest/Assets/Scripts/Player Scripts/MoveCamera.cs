using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        if(player != null)
            transform.position = player.transform.position;
    }
}
