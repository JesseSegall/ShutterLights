using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieState {
    Idle,
    Chase
}
// TODO: Add sound effects
public class ZombieBehavior : MonoBehaviour
{
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    private Transform player;
    private ZombieState state = ZombieState.Idle;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip zombieAggro;
    private bool hasPlayedChaseSound = false;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        switch (state)
        {
            case ZombieState.Idle:
                Idle();
                break;
            case ZombieState.Chase:
                // if (!hasPlayedChaseSound)
                // {
                //     PlayChaseSound();
                //     hasPlayedChaseSound = true;
                // }
                ChasePlayer();
                break;
        }
    }

    void Idle()
    {

        animator.SetBool("isChasing", false);

        // Check if the player is within detection range to switch to chase state.
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            state = ZombieState.Chase;
            // hasPlayedChaseSound = false;
        }
    }

    void ChasePlayer()
    {

        animator.SetBool("isChasing", true);

        float distance = Vector3.Distance(transform.position, player.position);

        // If within attack range, trigger the attack animation.
        if (distance <= attackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                // TODO: Add damage here
            }
            return; // Stop moving while attacking.
        }
        else
        {
            isAttacking = false;
            MoveTowards(player.position, chaseSpeed);

        }

        // If the player gets too far away, return to idle state.
        if (distance > detectionRange * 1.5f)
        {
            state = ZombieState.Idle;
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
            Debug.LogError("Hit Player");
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            if (lightBar != null)
            {
                lightBar.DamageTaken(5f);
            }
        }
    }

    // void PlayChaseSound()
    // {
    //     audioSource.PlayOneShot(zombieAggro);
    // }
}
