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

    // TODO:  Can possible remove this since its using root motion and I dont think chase speed overrides that
    public float chaseSpeed = 0.5f;
    public float detectionRange = 4f;
    // Not sure why but if nav stop dist is <= 1 it won't stop so attack range must be over 1
    public float attackRange = 1.01f;

    private readonly float ATTACK_ANIMATION_DURATION = 1.1f;

    [Header("Hand Collider Settings")]
    public GameObject handCollider;
    public float handColliderActivateTime = 0.3f;
    public float handColliderDeactivateTime = 0.3f;

    private Transform player;
    private ZombieState state = ZombieState.Idle;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip zombieAggro;
    private bool hasPlayedChaseSound = false;
    private NavMeshAgent navAgent;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = chaseSpeed;
        // Must make sure attack range is the same as nav stop distance
        navAgent.stoppingDistance = attackRange;


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
        // Debug.Log("Distance to player: " + distance + ", Attack Range: " + attackRange +
        //           ", Stopping Distance: " + navAgent.stoppingDistance);

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
        navAgent.isStopped = true;
        animator.SetBool("isChasing", false);

        if (distance < detectionRange)
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
            return;
        }

        // If within attack range, stop and begin attacking.
        if (distance <= attackRange)
        {
            navAgent.isStopped = true;
            // Rotate to face the player. Might need to adjust this somehow so its not so abrupt
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);

            state = ZombieState.Attacking;
            StartCoroutine(AttackRoutine());
            return;
        }


        animator.SetBool("isChasing", true);
        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);

        // Vector3 targetDir = (player.position - transform.position).normalized;
        // if (targetDir != Vector3.zero)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        // }
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

            // Wait until it's time to activate the hand collider.
            yield return new WaitForSeconds(handColliderActivateTime);
            ActivateHandCollider();

            // Wait until it's time to deactivate the hand collider.
            yield return new WaitForSeconds(handColliderDeactivateTime - handColliderActivateTime);
            DeactivateHandCollider();

            // Wait out the remainder of the attack animation.
            yield return new WaitForSeconds(ATTACK_ANIMATION_DURATION - handColliderDeactivateTime);

            // Make zombie look at player between attacks if they move
            // Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            // Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", true); // Ensure chase animation is activated.
        state = ZombieState.Chase;
        navAgent.isStopped = false;
        isAttacking = false;
        Debug.Log("Switching to Chase state");
    }
    // Have these so the collider only active when zombie is swinging at player so ideally they dont take damage by just running into the hand.
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

    // Some gizmos to visualize the ranges
    private void OnDrawGizmos()
    {
       // Detection range gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack Range gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Stop range gizmo though you wont see this since it should be the same as the attack range
        if (navAgent != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, navAgent.stoppingDistance);
        }
    }
}
