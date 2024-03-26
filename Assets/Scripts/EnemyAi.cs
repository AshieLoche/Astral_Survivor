using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public Animator animator; // Reference to the Animator component

    public Transform[] waypoints; // Array of waypoint transforms for patrolling
    public int currentWaypointIndex = -1; // Index of the current waypoint

    public float patrolSpeed = 2f; // Speed for patrolling
    public float idleDuration = 3f; // Time to idle at each waypoint
    private float idleTimer; // Timer for idling

    public float detectionRadius = 5f; // Radius for player detection
    public float chaseSpeed = 5f; // Speed for chasing the player

    private bool isChasing = false; // Flag to indicate chasing state

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>(); // Get Animator component
        agent.speed = patrolSpeed; // Set initial speed
    }

    void Start()
    {
        idleTimer = idleDuration; // Initialize idle timer
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer <= detectionRadius) // Player detected within radius
        {
            isChasing = true; // Start chasing the player
            agent.speed = chaseSpeed; // Set speed for chasing
            animator.SetBool("IsRunning", true); // Trigger running animation
            agent.SetDestination(player.position);
        }

        if (isChasing && distanceToPlayer <= detectionRadius)
        {
            agent.SetDestination(player.position);
        }

        if (isChasing && distanceToPlayer > detectionRadius) // Player exits radius
        {
            isChasing = false; // Stop chasing
            agent.speed = patrolSpeed; // Set speed back to patrol speed
            ResumePatrolling(); // Resume patrolling behavior
        }

        if (!isChasing) // Patrolling behavior
        {
            if (agent.remainingDistance <= agent.stoppingDistance) // Reached waypoint
            {
                animator.SetBool("IsWalking", false); // Trigger idle animation
                idleTimer -= Time.deltaTime; // Decrement idle timer

                if (idleTimer <= 0) // Idle time finished
                {
                    idleTimer = idleDuration; // Reset idle timer
                    NextWaypoint(); // Move to the next waypoint
                }
            }
            else // Moving to waypoint
            {
                animator.SetBool("IsWalking", true); // Trigger walking animation
            }
        }
    }

    void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop through waypoints
        agent.SetDestination(waypoints[currentWaypointIndex].position); // Set destination to next waypoint
    }

    void ResumePatrolling()
    {
        animator.SetBool("IsRunning", false); // Reset running animation
        NextWaypoint(); // Move to the next waypoint after stopping chase
    }
}
