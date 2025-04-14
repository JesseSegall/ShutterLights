using System.Collections;
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

    [Header("Animation Settings")]
   
    public float blendDampTime = 0f; 

    [Header("Rotation Settings")]
   
    public float rotationSpeed = 5f;
    
    [Header("Sound Settings")]
    public AudioSource audioSource;
    
    [Header("Aggro Sound Clip")]
    public AudioClip zombieAggro;
    void Awake()
    {
        // Find the player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("No Player game object found.");
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

       
        agent.updateRotation = true;
        
        // Set stopping distance to attack range
        agent.stoppingDistance = attackRange;
        
        // Set up NavMeshAgent for proper obstacle avoidance
        agent.radius = 0.4f;  
        agent.height = 2.0f;  
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        
        
        agent.avoidancePriority = Random.Range(40, 60);  
        
       
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<CapsuleCollider>();
            col.center = new Vector3(0, 1, 0);
            col.height = 2f;
            col.radius = 0.4f;
        }
        
        // Make is trigger true so they dont push the player around
        col.isTrigger = true;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned!");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

       
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
        else if (distance <= chaseRange)
        {
            // Chase 
            if (!_hasPlayedChaseSound)
            {
                audioSource.PlayOneShot(zombieAggro);
                _hasPlayedChaseSound = true;
                
            }
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("AnimBlend", 1f, blendDampTime, Time.deltaTime); // Run animation (1)
        }
        else
        {
            // Idle
            agent.isStopped = true;
            animator.SetFloat("AnimBlend", 0f, blendDampTime, Time.deltaTime); // Idle animation (0)
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

        // Always check if animation is done or not and play it again right away so there is no delay
        while (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
            {
                // As soon as the animation finishes, replay it right away so there is no awkward pause
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
                // When attacking, force zero movement
                agent.velocity = Vector3.zero;
            }
            else if (!agent.isStopped)
            {
                // Apply root motion to agent velocity
                Vector3 rootMotion = animator.deltaPosition;
                agent.velocity = rootMotion / Time.deltaTime;
            }
        }
    }

    
    void OnDrawGizmos()
    {
        
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}