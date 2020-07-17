using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Moevement
    [SerializeField] private float moveSpeed = 200;
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float maxSpeed = 20;
    private float threshold = 0.01f;
    [SerializeField] private float counterMovement = 0.175f;
    [SerializeField] private float maxSlopeAngle = 35f;

    //Jumping
    [SerializeField] private float jumpForce = 250f;
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;

    //Crouching
    [SerializeField] private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    [SerializeField] private Vector3 playerScale;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Camera Look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Input
    private float moveHorizontal, moveVertical;
    bool sprint, crouch, jump;

    //Assignables
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform orientation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        Inputs();
        Look();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Inputs()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");
        crouch = Input.GetKey(KeyCode.LeftControl);

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(moveHorizontal, moveVertical, mag);

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (moveHorizontal > 0 && xMag > maxSpeed) moveHorizontal = 0;
        if (moveHorizontal < 0 && xMag < -maxSpeed) moveHorizontal = 0;
        if (moveVertical > 0 && yMag > maxSpeed) moveVertical = 0;
        if (moveVertical < 0 && yMag < -maxSpeed) moveVertical = 0;

        //If holding jump && ready to jump, then jump
        if (readyToJump && jump) Jump();

        // Movement in air + Multipliers
        float multiplier = 1f, multiplierV = 1f;

        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        //Player Movement Forces
        rb.AddForce(orientation.transform.forward * moveVertical * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * moveHorizontal * moveSpeed * Time.deltaTime * multiplier);
    }

    //Find the velocity relative to where the player is looking
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    //Jump Function
    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    //Rotations
    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    //Counter Movement
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jump) return;

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    //Ground Detection
    private bool cancellingGrounded;
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }
}
