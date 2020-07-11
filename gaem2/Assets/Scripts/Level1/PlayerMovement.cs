using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    //Enum
    private enum State { idle, running, jumping, falling, crouching, hurt, climbing, climbSliding }
    private State state = State.idle;

    //Health Script
    ////////////////////////////////////////////////////////////
    const int n = 10;                                         //
    const int m = 20;                                         //
    public int maxHP = 100;                                   //
    public int currHP;                                        //
    public static int minHP;                                  //
                                                              //  
    public GameObject resScreen;                              //
    public healtBarScript hpBar;                              //
    ////////////////////////////////////////////////////////////

    //Variables;
    //Floats
    const float CheckRadius = .2f;
    private float moveHorizontal, moveVertical;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float hurtForce = 2f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [SerializeField] private float jumpForce = 8f;

    //Integers
    private int points;
    private int l;

    //Bools
    private bool facingRight = true;
    private bool isGrounded;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    //Declarations
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private Transform ceilingCheck, groundCheck;
    [SerializeField] private LayerMask groundObjects;
    [SerializeField] private LayerMask ladderObject;
    [SerializeField] private Text text;
    private Collider2D collide;
    [SerializeField] private AudioSource[] audioSRC;

    //Loading Components
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collide = GetComponent<Collider2D>();

        resScreen.SetActive(false);
    }

    //Initializing variables
    void Start()
    {
        currHP = maxHP;
        minHP = 0;
        hpBar.setMaxHealth(maxHP);
        points = 0;

        l = audioSRC.Length;
    }

    //Updates every second
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            takeDMG(20);

        }

        if (state != State.hurt)
        {
            //Inputs
            ProcessInputs();

        }

        AnimationState();
        animator.SetInteger("state", (int)state);

        //Flipping
        Flipping();

        IsDead();

    }

    void FixedUpdate()
    {
        //Check if Grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, CheckRadius, groundObjects);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        switch (c.tag)
        {
            case "Gem":
                Destroy(c.gameObject);
                points++;
                text.text = points.ToString();
                break;
            case "Cherry":
                Destroy(c.gameObject);
                currHP += m;
                hpBar.setHealth(currHP);
                break;
            case "Spikes":
                currHP -= n;
                hpBar.setHealth(currHP);
                break;
            case "Floor":
                resScreen.SetActive(true);
                gameObject.SetActive(false);
                break;
            case "NextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "EndLevel":
                SceneManager.LoadScene("EndScene");
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Enemy en = col.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                en.DeathAnimation();
                Jump();
                points += 5;
                text.text = points.ToString();
            }
            else
            {
                state = State.hurt;
            
                if(col.gameObject.transform.position.x > transform.position.x)
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                else
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                takeDMG(10);
            }

        }
    }

    private void ProcessInputs()
    {
        //Horizontal Movement
        moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal < 0)
        {
            rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
        }

        if (moveHorizontal > 0)
        {
            rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
        }

        LadderMovement();

        //Vertical Movement
        moveVertical = Input.GetAxis("Vertical");

        //Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        //Crouching
        if (Input.GetButtonDown("Crouch"))
        {

            state = State.crouching;
        }

    }

    private void LadderMovement()
    {
        if (collide.IsTouchingLayers(ladderObject))
        {
            if (moveVertical < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveVertical * moveSpeed);
                state = State.climbing;
            }

            if (moveVertical > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveVertical * moveSpeed);
                state = State.climbing;
            }
        }
    }

    //The Flipping itself
    private void Flipping()
    {
        if (moveHorizontal > 0 && !facingRight)
            Flip();
        else if (moveHorizontal < 0 && facingRight)
            Flip();
    }

    //Flip Character method
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    //Health Method
    public void IsDead()
    {
        if (currHP <= minHP)
        {
            for (int i = 0; i < l; i++)
                audioSRC[i].Stop();
            gameObject.SetActive(false);
            resScreen.SetActive(true);
        }
    }

    //Health Method
    void takeDMG(int hp)
    {
        currHP -= hp;
        hpBar.setHealth(currHP);
    }

    //Jump method
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        audioSRC[Random.Range(0, l)].Play();
        state = State.jumping;
    }

    //State method
    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.001f)
            {
                state = State.falling;
            }
        }

        else if (state == State.falling)
        {
            if (isGrounded)
            {
                state = State.idle;
            }
        }

        else if(state == State.hurt)
        {
            if (Math.Abs(rb.velocity.x) < 0.01f)
                state = State.idle;
        }

        else if (Math.Abs(moveHorizontal) > 0.01f)
        {
            state = State.running;
        }
        
        else if (state == State.climbing)
        {
            if (rb.velocity.y < 0.01f)
                state = State.climbSliding;

            if (!collide.IsTouchingLayers(ladderObject) && !isGrounded)
            {
                state = State.falling;
            }
        }

        else
        {
            state = State.idle;
        }
    }
}
