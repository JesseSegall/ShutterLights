using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ZombieState {
    Idle,
    Chase,
    Attacking
}

public class ZombieBehavior : MonoBehaviour
{
    [Header("Chase Settings")]
    // TODO:  Can possibly remove this since its using root motion and I dont think chase speed overrides that
    public float chaseSpeed = 0.5f;
    public float detectionRange = 4f;
    // Not sure why but if nav stop dist is <= 1 it won't stop so attack range must be over 1
    public float attackRange = 1.01f;

    private readonly float ATTACK_ANIMATION_DURATION = 1.1f;

    [Header("Other Settings")]

    public float maxAggroDelay = 3f;

    private Transform player;
    private ZombieState state = ZombieState.Idle;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip zombieAggro;
    private bool hasPlayedChaseSound = false;
    private NavMeshAgent navAgent;
    private bool isAttacking = false;

    private float aggroDelay;
    private float spawnTime;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = chaseSpeed;
        // Must make sure attack range is the same as nav stop distance
        navAgent.stoppingDistance = attackRange;

        // Make some random delay so when in a pack they all dont have exact same animation times
        aggroDelay = Random.Range(0f, maxAggroDelay);
        spawnTime = Time.time;

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isAttacking)
            return;

        switch (state)
        {
            case ZombieState.Idle:
                HandleIdleState(distance);
                break;
            case ZombieState.Chase:
                HandleChaseState(distance);
                break;
        }
    }

    void HandleIdleState(float distance)
    {
        Debug.Log("Idle State: distance = " + distance + ", spawnTime + aggroDelay = " + (spawnTime + aggroDelay) + ", Time.time = " + Time.time);

        navAgent.isStopped = true;
        animator.SetBool("isChasing", false);

        if (distance < detectionRange && Time.time >= spawnTime + aggroDelay)
        {
            state = ZombieState.Chase;
            if (!hasPlayedChaseSound && zombieAggro != null)
            {
                audioSource.PlayOneShot(zombieAggro);
                hasPlayedChaseSound = true;
            }
        }
    }

    void HandleChaseState(float distance)
    {
        // If the player gets too far out of detection range go back to idle state
        if (distance > detectionRange * 1.5f)
        {
            state = ZombieState.Idle;
            hasPlayedChaseSound = false;
            spawnTime = Time.time;
            aggroDelay = Random.Range(1f, maxAggroDelay);
            return;
        }

        // If within attack range, stop and begin attacking.
        if (distance <= attackRange)
        {
            navAgent.isStopped = true;
            // Rotate to face the player.
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);

            state = ZombieState.Attacking;
            StartCoroutine(AttackRoutine());
            return;
        }

        animator.SetBool("isChasing", true);
        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);
    }

    IEnumerator AttackRoutine()
    {
        Debug.Log("In attack routine");
        isAttacking = true;
        animator.SetBool("isAttacking", true);



        // Continuously attack while the player is within range.
        while (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Restart the attack animation from the beginning.
            animator.Play("Attack", 0, 0f);
            // Wait for the full duration of the attack animation.
            yield return new WaitForSeconds(ATTACK_ANIMATION_DURATION);

            // This is causing some weird issues so commented out for now
            // Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            // Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }

        // When the player moves out of range, exit attack state.
        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", true);
        state = ZombieState.Chase;
        navAgent.isStopped = false;
        isAttacking = false;


        Debug.Log("Switching to Chase state");
    }



    // Some gizmos to visualize the ranges
    private void OnDrawGizmos()
    {
        // Detection range gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack Range gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Stop range gizmo (should be the same as attack range)
        if (navAgent != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, navAgent.stoppingDistance);
        }
    }
}
