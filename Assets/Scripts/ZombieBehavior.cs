using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ZombieState {
    Idle,
    Chase
}

public class ZombieBehavior : MonoBehaviour
{
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    private readonly float ATTACK_ANIMATION_DURATION = 1.1f; // Attack animation length

    // Hand collider settings
    public GameObject handCollider;
    public float handColliderActivateTime = 0.3f;
    public float handColliderDeactivateTime = 0.8f;

    private Transform player;
    private ZombieState state = ZombieState.Idle;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip zombieAggro;
    private bool hasPlayedChaseSound = false;

    private bool isAttacking = false;
    private NavMeshAgent navAgent;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = chaseSpeed;
        navAgent.stoppingDistance = attackRange * 0.8f;

        // Disable hand collider at start
        if (handCollider != null)
        {
            handCollider.SetActive(false);
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // If currently in attack animation, don't do anything else
        if (isAttacking)
            return;

        switch (state)
        {
            case ZombieState.Idle:
                Idle(distance);
                break;
            case ZombieState.Chase:
                ChasePlayer(distance);
                break;
        }
    }

    void Idle(float distance)
    {
        // Stop movement and animation
        navAgent.isStopped = true;
        animator.SetBool("isChasing", false);

        // Check if player is within detection range
        if (distance < detectionRange)
        {
            state = ZombieState.Chase;
        }
    }

    void ChasePlayer(float distance)
    {
        // If player gets too far, go back to idle
        if (distance > detectionRange * 1.5f)
        {
            state = ZombieState.Idle;
            return;
        }

        // If within attack range, attack
        if (distance <= attackRange)
        {
            // Stop moving
            navAgent.isStopped = true;

            // Look at player
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);

            // Start attack
            isAttacking = true;
            animator.SetBool("isChasing", false);
            animator.SetTrigger("Attack");

            // Schedule hand collider activation during attack
            Invoke("ActivateHandCollider", handColliderActivateTime);
            Invoke("DeactivateHandCollider", handColliderDeactivateTime);

            // Schedule end of attack
            Invoke("FinishAttack", ATTACK_ANIMATION_DURATION);
            return;
        }

        // Continue chasing
        navAgent.isStopped = false;
        animator.SetBool("isChasing", true);
        navAgent.SetDestination(player.position);
    }

    void ActivateHandCollider()
    {
        if (handCollider != null)
        {
            handCollider.SetActive(true);
        }
    }

    void DeactivateHandCollider()
    {
        if (handCollider != null)
        {
            handCollider.SetActive(false);
        }
    }

    void FinishAttack()
    {
        isAttacking = false;

        // Ensure hand collider is deactivated
        DeactivateHandCollider();
    }


}