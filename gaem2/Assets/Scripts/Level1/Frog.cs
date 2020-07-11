using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    private bool facingRight = true;
    private PlayerMovement p;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   //Flip Character method
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
