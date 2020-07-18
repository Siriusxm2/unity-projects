using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float maxDistance = 12f;
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    public static bool enemyShooting = false;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (agent.enabled) {
            float dist = Vector3.Distance(player.transform.position, this.transform.position);
            Vector3 lookAtPos = transform.position - player.gameObject.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 1);
            if (dist < maxDistance)
                enemyShooting = true;
            else
                enemyShooting = false;
        }
    }
}
