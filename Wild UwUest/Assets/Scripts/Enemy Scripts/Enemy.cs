using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Bools
    public static bool shooting = false;
    //private bool playerSighted = false;

    // Assignables
    private GameObject target = null;
    private NavMeshAgent agent;
   // [SerializeField] private AudioSource audioSRC;

    // Enemy Movement
    [SerializeField] private float moveSpeed;
    private float minDist = 3;
    private float maxDist = 9;

    // Patrol
    public GameObject[] waypoints;
    private int wayPointInd;
    public float patrolSpeed;

    // Investigate
    [HideInInspector] public Vector3 invSpot;
    private float timer = 0;
    [SerializeField] private float invWait = 5f;

    // Sight
    private float height;
    [SerializeField] private float sightDist = 10f;

    public enum State{
        PATROL,
        CHASE,
        INVESTIGATE
    }
    public State state;
    private bool alive;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
       // audioSRC = GetComponent<AudioSource>();
    }

    private void Start()
    {
        agent.updatePosition = true;
        agent.updateRotation = false;
        target = GameObject.Find("Player");
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        wayPointInd = Random.Range(0, waypoints.Length);

        state = Enemy.State.PATROL;

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
                case State.PATROL:
                    Patrol();
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
            float dis = Vector3.Distance(this.transform.position, target.transform.position);
            turnToLook();
            if (dis >= minDist) {
                agent.speed = moveSpeed * Time.deltaTime;
                agent.SetDestination(target.transform.position);
                if (dis <= maxDist) {
                    shooting = true;
                }
            }
            if (dis < minDist) {
                agent.speed = 0f;
                agent.SetDestination(this.transform.position);
                shooting = true;
            }
        } else {
            shooting = false;
            state = Enemy.State.PATROL;
        }

    }

    private void Patrol()
    {
        agent.speed = patrolSpeed * Time.deltaTime;
        shooting = false;
        if (Vector3.Distance(this.transform.position, waypoints[wayPointInd].transform.position) >= 2) {
            agent.SetDestination(waypoints[wayPointInd].transform.position);
            transform.LookAt(2 * transform.position - waypoints[wayPointInd].transform.position);
        } else if (Vector3.Distance(this.transform.position, waypoints[wayPointInd].transform.position) <= 2) {
            wayPointInd = Random.Range(0, waypoints.Length);
        } else
            agent.SetDestination(this.transform.position);

    }

    private void Investigate() {
        timer += Time.deltaTime;
        agent.SetDestination(invSpot);
        agent.speed = moveSpeed * Time.deltaTime;
        transform.LookAt(2 * transform.position - invSpot);
        if(timer >= invWait) {
            state = Enemy.State.PATROL;
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
            invSpot = target.transform.position;
            state = Enemy.State.INVESTIGATE;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            invSpot = target.transform.position;
            state = Enemy.State.INVESTIGATE;
        }
    }

    private void turnToLook()
    {
        Vector3 lookAtPos = transform.position - target.gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 0.75f);
        //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 1);
        //lookAtPos.y = transform.position.y;
        // transform.LookAt(2 * transform.position - lookAtPos);
    }
}
