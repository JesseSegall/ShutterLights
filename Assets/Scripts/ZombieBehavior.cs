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
    public float maxAggroDelay = 10f;

    private Transform player;
    private ZombieState state = ZombieState.Idle;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip zombieAggro;
    private bool hasPlayedChaseSound = false;
    private NavMeshAgent navAgent;
    private bool isAttacking = false;


    private float aggroDelay;
    //When player is detected, use -1 as default
    private float detectionStartTime = -1f;

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
        aggroDelay = Random.Range(0.5f, maxAggroDelay);

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
            case ZombieState.Attacking:
                // Attacking is handled in the AttackRoutine coroutine.
                break;
        }
    }

    void HandleIdleState(float distance)
    {
        navAgent.isStopped = true;
        animator.SetBool("isChasing", false);

        if (distance < detectionRange)
        {
            // When the player is first detected, start the delay timer.
            if (detectionStartTime < 0f)
            {
                detectionStartTime = Time.time;
            }
            // If the delay has passed, transition to chase state.
            if (Time.time >= detectionStartTime + aggroDelay)
            {
                state = ZombieState.Chase;
                if (!hasPlayedChaseSound && zombieAggro != null)
                {
                    audioSource.PlayOneShot(zombieAggro);
                    hasPlayedChaseSound = true;
                }
            }
        }
        else
        {
            // Reset the detection timer if the player leaves the detection range.
            detectionStartTime = -1f;
        }
    }

    void HandleChaseState(float distance)
    {
        // If the player gets too far, revert back to Idle state.
        if (distance > detectionRange * 1.5f)
        {
            state = ZombieState.Idle;
            hasPlayedChaseSound = false;
            // Reset aggro delay and detection timer for future detections.
            aggroDelay = Random.Range(0.3f, maxAggroDelay);
            detectionStartTime = -1f;
            return;
        }

        // If within attack range, stop and begin attacking.
        if (distance <= attackRange)
        {
            navAgent.isStopped = true;
            // TODO: Need to make this rotation more smooth
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
        }

        // Exit the attack state when the player moves out of range.
        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", true);
        state = ZombieState.Chase;
        navAgent.isStopped = false;
        isAttacking = false;

        Debug.Log("Switching to Chase state");
    }

    // Gizmos to visualize detection and attack ranges.
    private void OnDrawGizmos()
    {
        // Detection range gizmo.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range gizmo.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Stopping distance gizmo.
        if (navAgent != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, navAgent.stoppingDistance);
        }
    }
}
