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
    public float detectionRange = 4f;
    public float stoppingDistance = 2f;
    private Transform player;
    private AIState state = AIState.Patrol;
    private int currentWaypointIndex = 0; 
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip ghostAggro;
    public AudioClip death;
    private SkinnedMeshRenderer meshRenderer;
    private bool hasPlayedChaseSound = false;
    private bool isDying = false;



    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; 
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (audioSource == null)
    {
        Debug.LogError("AudioSource is missing on " + gameObject.name);

    }
        Material material = meshRenderer.sharedMaterial;
        Color resetColor = material.color;
        resetColor.a = 1f; 
        material.color = resetColor;
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (isDying) return;
        switch (state)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                if (!hasPlayedChaseSound)
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
       // animator.SetFloat("Speed", speed);

       
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
        //animator.SetFloat("Speed", speed);

       
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
            Debug.LogError("Hit Player");
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            //LightDecay lightArea = other.GetComponentInChildren<LightDecay>();
            lightBar.DamageTaken(2f);
            //lightArea.GhostContactAreaLight(2f);
            Debug.LogError("Right before isDying");
            isDying = true;
            PlayDeathSound();
            animator.SetTrigger("Death");
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    void PlayChaseSound(){
        audioSource.PlayOneShot(ghostAggro);
    }

    void PlayDeathSound(){
        Debug.LogError("Reached PlayDeathSound");
        audioSource.PlayOneShot(death);
    }

IEnumerator FadeOutGhost(float duration)
    {
        float elapsedTime = 0f;
        Material material = meshRenderer.material;
        Color startColor = material.color;

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / duration);
            material.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        gameObject.SetActive(false);
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(FadeOutGhost(1f)); 
    }
}
