using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossState {
    Throw,
    Idle,
    Spin,
    Death
}

public class BossBehavior : MonoBehaviour
{

    private Transform player;
    public GameObject bossProjectile;
    private NavMeshAgent projectileAgent;
    private BossState state = BossState.Idle;
    public int bossHealth = 100;
    private Animator animator;
    private bool isThrowing = false;
    private bool isDead = false;
    public GameObject youWinCanvas;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (bossHealth <= 0)
        {
            OnDeath();
            return;
        }
        if (bossHealth <= 50 && state != BossState.Spin)
        {
            state = BossState.Spin;
        }
        switch(state)
        {
            case BossState.Idle:
            break;
            case BossState.Throw:
                if(!isThrowing) {
                    StartCoroutine(ThrowState());
                }
            
            if (state != BossState.Spin && bossHealth > 50)
                {
                    state = BossState.Idle;
                    StartCoroutine(IdleState());
                }
            break;
            case BossState.Spin:
            SpinState();
            break;
            
        }
    }

    private IEnumerator ThrowState()
{
    state = BossState.Throw; // Set state to Throw
    isThrowing = true;
    animator.SetTrigger("Throw");

    // Wait for the animation to finish (adjust based on animation length)
    yield return new WaitForSeconds(3f);

    // Create the projectile AFTER animation delay
    GameObject newProjectile = Instantiate(bossProjectile, transform.position + transform.forward * 12f + Vector3.up * 20f, Quaternion.identity);
    newProjectile.GetComponent<MeshRenderer>().enabled = true;

    // Use Rigidbody for movement
    Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
    Vector3 direction = (player.transform.position - newProjectile.transform.position).normalized;
    float projectileSpeed = 30f;
    rb.velocity = direction * projectileSpeed;

    animator.ResetTrigger("Throw");
    isThrowing = false;
    // Change state after the throw
    if (bossHealth <= 50)
    {
        state = BossState.Spin;
    }
    else
    {
        state = BossState.Idle;
        StartCoroutine(IdleState());
    }
}

    private IEnumerator IdleState(){
        yield return new WaitForSeconds(1f);
        if (state != BossState.Spin && !isThrowing) 
        {
            state = BossState.Throw;
        }
    }


    private void SpinState(){
        MoveTowards(player.position, 1f);
        

        
    }

    void MoveTowards(Vector3 target, float speed)
    {
        animator.SetTrigger("Spinning");
        Vector3 ylessTarget = new Vector3(target.x, transform.position.y, target.z);
    
        Vector3 direction = (ylessTarget - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        //transform.LookAt(ylessTarget);
        transform.Rotate(0f, 100f * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {

            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            LightDecay lightArea = other.GetComponentInChildren<LightDecay>();
            lightBar.GhostContact(2f);
            lightArea.GhostContactAreaLight(2f);
        }
    }


    private void OnDeath()
    {
        if (isDead) return; 

        isDead = true;
        state = BossState.Death; 
        animator.SetTrigger("Death");
        if (youWinCanvas != null) {
            youWinCanvas.SetActive(true);
        }

        Debug.Log("Boss has died!");

    }
}
