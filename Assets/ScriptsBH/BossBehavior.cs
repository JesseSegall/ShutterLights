using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum BossState {
    Throw,
    Idle,
    Spin,
    Death
}

public class BossBehavior : MonoBehaviour
{

    public Transform orbFirePoint;
    private Transform player;
    public GameObject bossProjectile;
    private NavMeshAgent projectileAgent;
    private BossState state = BossState.Idle;
    public float bossHealth = 100f;
    private Animator animator;
    float projectileSpeed = 15f;
    private bool isThrowing = false;
    private bool isDead = false;
    public GameObject youWinCanvas;
    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(IdleState());
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
        if (isDead) return;

        LookAtPlayer(3f);

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
        state = BossState.Throw;
        isThrowing = true;
        animator.SetTrigger("Throw");
        // if (isDead || bossHealth <= 0 || orbFirePoint == null) return;
        Debug.Log("Firing Projectile");
        yield return new WaitForSeconds(2f);
        Quaternion lookRotation = Quaternion.LookRotation(player.position - orbFirePoint.position);
        GameObject newProjectile = Instantiate(bossProjectile, orbFirePoint.position, lookRotation);
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
        if (player != null && rb != null)
        {
            Vector3 playerTarget = new Vector3(player.position.x, player.position.y + 2.0f, player.position.z);
            Vector3 direction = (playerTarget - orbFirePoint.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }
        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("Throw");
        isThrowing = false;

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
        MoveTowards(player.position, 2f);



    }

    void LookAtPlayer(float turnSpeed)
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Keeps boss upright, prevents tilting
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
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
           // LightDecay lightArea = other.GetComponentInChildren<LightDecay>();
            lightBar.DamageTaken(2f);
            //lightArea.GhostContactAreaLight(2f);
            UpdateHealthBar();
        }
    }


    private void OnDeath()
    {
        if (isDead) return;

        isDead = true;
        state = BossState.Death;
        animator.SetTrigger("Death");
        SceneManager.LoadScene("EndScene");


    }

    void UpdateHealthBar()
    {
            healthBar.fillAmount = bossHealth / 100f;
    }
}
