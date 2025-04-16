using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieTest : MonoBehaviour
{
    [Header("References")]
    public Transform player;  

    private Animator animator;
    private NavMeshAgent agent;

    [Header("AI Ranges")]
    public float chaseRange = 10f;
    public float attackRange = 2.5f;

    private bool _isAttacking = false;
    private bool _hasPlayedChaseSound = false;
    private bool _hasDetectedPlayer = false;
    private Vector3 _zombieSpawnPosition;

    [Header("Animation Settings")]
    public float blendDampTime = 0f; 

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    
    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;

    [Header("Aggro Sound Clip")]
    [SerializeField] private AudioClip zombieAggro;

    void Awake()
    {
        // Find the player if not assigned.
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("No Player game object found.");
        }
    }

    void OnEnable()
    {
        // Sub to the event
        PlayerDeath.OnPlayerRespawn += ResetZombie;
    }

    void OnDisable()
    {
        // Un sub from event so don't leak memory
        PlayerDeath.OnPlayerRespawn -= ResetZombie;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        // Save the original spawn position of the zombie
        _zombieSpawnPosition = transform.position;
        Debug.Log(gameObject.name + " recorded spawn at: " + _zombieSpawnPosition);

        agent.updateRotation = true;
        // Set stopping distance to attack range.
        agent.stoppingDistance = attackRange;
        
        // Configure the NavMeshAgent for proper obstacle avoidance.
        agent.radius = 0.4f;  
        agent.height = 2.0f;  
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(40, 60);  
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned!");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= chaseRange)
        {
            _hasDetectedPlayer = true;
        }

        if (_isAttacking)
        {
            agent.isStopped = true;
            LookAtTarget(player.position);
            return;
        }

        if (distance <= attackRange)
        {
            // Attack
            agent.isStopped = true;
            StartCoroutine(ContinuousAttackLoop());
        }
        else if (_hasDetectedPlayer)
        {
            // Chase
            if (!_hasPlayedChaseSound)
            {
                audioSource.PlayOneShot(zombieAggro);
                Debug.Log("Playing aggro sound: " + zombieAggro);
                _hasPlayedChaseSound = true;
            }
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("AnimBlend", 1f, blendDampTime, Time.deltaTime); // Run animation is 1 in the blend tree.
        }
        else
        {
            // Idle
            agent.isStopped = true;
            animator.SetFloat("AnimBlend", 0f, blendDampTime, Time.deltaTime); // Idle animation is 0 in the blend tree.
        }

        if (agent.hasPath && !agent.isStopped)
        {
            LookAtTarget(agent.steeringTarget);
        }
    }
    
    void LookAtTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator ContinuousAttackLoop()
    {
        _isAttacking = true;
        animator.SetBool("isAttacking", true);
        
        animator.Play("Attack", 0, 0f);
        Debug.Log("Started Attack Loop");

        // Continuously replay the attack animation as long as within attack range.
        while (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
            {
                // Replay the attack animation immediately after it finishes.
                animator.Play("Attack", 0, 0f);
                Debug.Log("Re-triggering Attack");
            }
            yield return null; 
        }

        animator.SetBool("isAttacking", false);
        _isAttacking = false;
        Debug.Log("Exiting Attack Loop, switching back to Chase");
        yield break;
    }
    
    void OnAnimatorMove()
    {
        if (animator)
        {
            if (_isAttacking)
            {
                // When attacking, force zero movement.
                agent.velocity = Vector3.zero;
            }
            else if (!agent.isStopped)
            {
                // Apply root motion to the agent's velocity.
                Vector3 rootMotion = animator.deltaPosition;
                agent.velocity = rootMotion / Time.deltaTime;
            }
        }
    }

    
    void ResetZombie()
    {
       // Debug.Log(gameObject.name + " resetting to spawn position: " + _zombieSpawnPosition);
        if (agent != null)
        {
            
            // Need to use warp because just setting transform.position is seemingly too slow for some zombies.
            agent.Warp(_zombieSpawnPosition);
            agent.ResetPath();
            agent.isStopped = true;
        }
        // Ensure the transform position is set (Warp should update this, too).
        transform.position = _zombieSpawnPosition;
        animator.SetFloat("AnimBlend", 0f, blendDampTime, Time.deltaTime);
        _hasDetectedPlayer = false;
        _isAttacking = false;
        _hasPlayedChaseSound = false;
    }
    
    void OnDrawGizmos()
    {
        // Uncomment the following lines if you wish to visualize the chase range.
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
