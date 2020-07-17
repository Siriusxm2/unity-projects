using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    public static bool enemyShooting = false;
    [SerializeField] private GameObject target;

    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private float health = 100f;
    private bool playerSighted = false;
    private float minDist = 10;
    private float maxDist = 15;

    public Rigidbody rb;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AudioSource audioSRC;

    public enum State{
        IDLE,
        CHASE 
}
    public State state;
    private bool alive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        audioSRC = GetComponent<AudioSource>();
    }

    private void Start()
    {
        agent.updatePosition = true;
        agent.updateRotation = false;

        state = Enemy.State.IDLE;

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
            }
            yield return null;
        }
    }

    private void Chase()
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) >= minDist)
        {
            agent.speed = moveSpeed;
            agent.SetDestination(target.transform.position);
            if (Vector3.Distance(this.transform.position, target.transform.position) <= maxDist)
                enemyShooting = true;
        }
    }

    private void Idle()
    {
        agent.speed = 0;
        agent.SetDestination(this.transform.position);

        
    }

    private void Update()
    {
        if(playerSighted == true)
        {
            Chase();
            turnToLook();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerSighted = true;
            audioSRC.Play();
            state = Enemy.State.CHASE;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerSighted = false;
            enemyShooting = false;
            state = Enemy.State.IDLE;
        }
    }

    private void turnToLook()
    {
        Vector3 lookAtPos = transform.position - target.gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos), 1);
        //lookAtPos.y = transform.position.y;

       // transform.LookAt(2 * transform.position - lookAtPos);


    }

    public void takeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Dies();
        }
    }

    private void Dies()
    {
        Destroy(gameObject);
    }
}
