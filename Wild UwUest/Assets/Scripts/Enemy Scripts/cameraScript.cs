using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class cameraScript : MonoBehaviour {

    // Bools
    public static bool shooting = false;

    // Assignables
    private NavMeshAgent agent;
    [HideInInspector] public GameObject target = null;

    // Enemy Movement
    [SerializeField] private float moveSpeed;
    private float minDist = 3;
    private float maxDist = 9;

    // Patrol
    public GameObject[] waypoints;
    private int wayPointInd;
    private float patrolSpeed;

    // Camera Sight
    public GameObject player;
    public Collider playerColl;
    public Camera enemyCam;
    private Plane[] planes;

    public enum State {
        PATROL,
        CHASE
    }
    public State state;
    private bool alive;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        agent.updatePosition = true;
        agent.updateRotation = false;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        wayPointInd = Random.Range(0, waypoints.Length);

        player = GameObject.Find("Player").gameObject;

        state = cameraScript.State.PATROL;

        patrolSpeed = moveSpeed / 2;
        alive = true;

        StartCoroutine("FSM");
    }

    IEnumerator FSM() {
        while (alive) {
            switch (state) {
                case State.PATROL:
                    Patrol();
                    break;
                case State.CHASE:
                    Chase();
                    break;
            }
            yield return null;
        }
    }

    private void Chase() {

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
        } else {
            shooting = false;
            state = cameraScript.State.PATROL;
        }

    }

    private void Patrol() {
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

    private void turnToLook() {
        Vector3 lookAtPos = transform.position - target.gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 0.75f);
        //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 1);
        //lookAtPos.y = transform.position.y;
        // transform.LookAt(2 * transform.position - lookAtPos);
    }

    private void Update() {
        planes = GeometryUtility.CalculateFrustumPlanes(enemyCam);
        if (GeometryUtility.TestPlanesAABB(planes, playerColl.bounds)) {
            CheckForPlayer();
        } else {
            state = cameraScript.State.PATROL;
        }
    }

    void CheckForPlayer() {
        RaycastHit hit;
        Debug.DrawRay(enemyCam.transform.position, -transform.forward * 10, Color.green);
        if(Physics.Raycast(enemyCam.transform.position, -transform.forward, out hit, 10)) {
            state = cameraScript.State.CHASE;
            target = hit.collider.gameObject;
        }
    }
}
