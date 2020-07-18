using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Bools
    public static bool shooting = false;
    //private bool playerSighted = false;

    // Assignables
    private GameObject target;
    private NavMeshAgent agent;
    [SerializeField] private AudioSource audioSRC;

    // Enemy Movement
    [SerializeField] private float moveSpeed;
    private float minDist = 10;
    private float maxDist = 15;

    // Investigate
    private Vector3 invSpot;
    private float timer = 0;
    [SerializeField] private float invWait = 5f;

    // Sight
    [SerializeField] private float height;
    [SerializeField] private float sightDist = 12f;

    public enum State{
        IDLE,
        CHASE,
        INVESTIGATE
    }
    public State state;
    private bool alive;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSRC = GetComponent<AudioSource>();
    }

    private void Start()
    {
        agent.updatePosition = true;
        agent.updateRotation = false;
        target = GameObject.Find("Player");

        state = Enemy.State.IDLE;

        height = 0.488f;
        alive = true;

        StartCoroutine("FSM");
    }

    IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.IDLE:
                    Idle();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.INVESTIGATE:
                    Investigate();
                    break;
            }
            yield return null;
        }
    }

    private void Chase()
    {
        if (target != null) {
            audioSRC.Play();
            turnToLook();
            if (Vector3.Distance(this.transform.position, target.transform.position) >= minDist) {
                agent.speed = moveSpeed * Time.deltaTime;
                agent.SetDestination(target.transform.position);
                if (Vector3.Distance(this.transform.position, target.transform.position) <= maxDist) {
                    shooting = true;
                }
            }
        } else {
            shooting = false;
            state = Enemy.State.IDLE;
        }

    }

    private void Idle()
    {
        agent.speed = 0f;
        agent.SetDestination(this.transform.position);
        shooting = false;
    }

    private void Investigate() {
        timer += Time.deltaTime;
        agent.SetDestination(this.transform.position);
        agent.speed = 0f;
        transform.LookAt(2 * transform.position - invSpot);
        if(timer >= invWait) {
            state = Enemy.State.IDLE;
            timer = 0;
        }
    }

    private void FixedUpdate() {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * height, -transform.forward * sightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * height, (-transform.forward + transform.right).normalized * sightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * height, (-transform.forward - transform.right).normalized * sightDist, Color.green);
        if(Physics.Raycast(transform.position + Vector3.up * height, -transform.forward, out hit, sightDist)) {
            if(hit.collider.gameObject.tag == "Player" && hit.collider.gameObject.tag != "Wall") {
                state = Enemy.State.CHASE;
                
            }
        }
        if (Physics.Raycast(transform.position + Vector3.up * height, (-transform.forward + transform.right).normalized, out hit, sightDist)) {
            if (hit.collider.gameObject.tag == "Player" && hit.collider.gameObject.tag != "Wall") {
                state = Enemy.State.CHASE;

            }
        }
        if (Physics.Raycast(transform.position + Vector3.up * height, (-transform.forward - transform.right).normalized, out hit, sightDist)) {
            if (hit.collider.gameObject.tag == "Player" && hit.collider.gameObject.tag != "Wall") {
                state = Enemy.State.CHASE;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            invSpot = target.transform.forward;
            state = Enemy.State.INVESTIGATE;
            Debug.Log("Entered");
            
        }
    }

    private void turnToLook()
    {
        Vector3 lookAtPos = transform.position - target.gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 1f);
        //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 1);
        //lookAtPos.y = transform.position.y;
        // transform.LookAt(2 * transform.position - lookAtPos);
    }
}
