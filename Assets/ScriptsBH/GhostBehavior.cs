using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState {
    Patrol,
    Chase
}

public class GhostBehavior : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;
    public float stoppingDistance = 2f;

    private Transform player;
    private AIState state = AIState.Patrol;
    private int currentWaypointIndex = 0; 
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip ghostAggro;
    public AudioClip death;

    private bool hasPlayedChaseSound = false;


    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; 
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
    {
        Debug.LogError("AudioSource is missing on " + gameObject.name);
    }
        
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        switch (state)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                if (!hasPlayedChaseSound) // Play sound only once per chase state
            {
                PlayChaseSound();
                hasPlayedChaseSound = true;
            }
                ChasePlayer();
                break;
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        float speed = moveSpeed;
        MoveTowards(waypoints[currentWaypointIndex].position, speed);
        animator.SetFloat("Speed", speed);

       
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < stoppingDistance)
        {
            
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

       
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            state = AIState.Chase;
            hasPlayedChaseSound = false;
            

        }
    }

    void ChasePlayer()
    {
        
        float speed = chaseSpeed;
        MoveTowards(player.position, speed);
        animator.SetFloat("Speed", speed);

       
        if (Vector3.Distance(transform.position, player.position) > detectionRange * 1.5f)
        {
            state = AIState.Patrol;
        }
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

      
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayDeathSound();
            Invoke("DisableGhost", death.length);
        }
    }

    void PlayChaseSound(){
        audioSource.PlayOneShot(ghostAggro);
    }

    void PlayDeathSound(){
        audioSource.PlayOneShot(death);
    }

    void DisableGhost()
{
    gameObject.SetActive(false);
}
}
