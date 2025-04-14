using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BossState {
    Throw,
    Idle,
    Run,
    Stomp,
    Death
}

public class BossBehavior : MonoBehaviour
{
    [Header("References")]
    public Transform orbFirePoint;
    public GameObject bossProjectile;
    public Image healthBar;

    [Tooltip("An empty GameObject at the boss's foot for stomp range checks.")]
    public Transform footPoint;

    [Header("Boss Settings")]
    public float bossHealth = 100f;
    public float projectileSpeed = 15f;
    public float runSpeed = 3f;
    public float closeRange = 5f;
    public float stompRange = 2f;
    public float stompCooldown = 2f;
    public float floorY = 0f;

    private BossState state = BossState.Idle;
    private bool isThrowing = false;
    private bool isDead = false;
    private bool isStomping = false;
    private float lastStompTime = -9999f;

    private Animator animator;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(IdleState());
        UpdateHealthBar();
    }

    void Update()
    {
        if (isDead) return;
        UpdateHealthBar();
        LookAtPlayer(3f);

        if (bossHealth <= 0)
        {
            OnDeath();
            return;
        }

        // ----------- DECISION LOGIC -------------
        if (bossHealth > 50)
        {
            if (state != BossState.Throw && !isThrowing)
            {
                if (state != BossState.Idle)
                {
                    state = BossState.Idle;
                    StartCoroutine(IdleState());
                }
            }
        }
        else
        {
            if (!IsPlayerOnFloor())
            {
                if (state != BossState.Throw && !isThrowing)
                {
                    if (state != BossState.Idle)
                    {
                        state = BossState.Idle;
                        StartCoroutine(IdleState());
                    }
                }
            }
            else
            {
                if (state != BossState.Run && state != BossState.Stomp)
                {
                    state = BossState.Run;
                }
            }
        }

        // ------------ STATE EXECUTION ------------
        switch (state)
        {
            case BossState.Idle:
                IdleLogic();
                break;
            case BossState.Throw:
                if (!isThrowing)
                    StartCoroutine(ThrowState());
                break;
            case BossState.Run:
                RunState();
                break;
            case BossState.Stomp:
                break;
            case BossState.Death:
                break;
        }
    }

    private bool IsPlayerOnFloor()
    {
        float margin = 0.1f;
        return (player.position.y <= floorY + margin);
    }

    private IEnumerator IdleState()
    {
        yield return new WaitForSeconds(1f);
        if (bossHealth > 50 && state == BossState.Idle && !isThrowing)
        {
            state = BossState.Throw;
        }
        else if (bossHealth <= 50 && !IsPlayerOnFloor() && state == BossState.Idle && !isThrowing)
        {
            state = BossState.Throw;
        }
    }

    private IEnumerator ThrowState()
    {
        state = BossState.Throw;
        isThrowing = true;
        animator.SetTrigger("Throw");

        yield return new WaitForSeconds(2f);

        if (orbFirePoint != null && bossProjectile != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(player.position - orbFirePoint.position);
            GameObject newProjectile = Instantiate(bossProjectile, orbFirePoint.position, lookRotation);

            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 playerTarget = new Vector3(player.position.x, player.position.y + 2.0f, player.position.z);
                Vector3 direction = (playerTarget - orbFirePoint.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }
        }

        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("Throw");
        isThrowing = false;

        if (bossHealth <= 50 && IsPlayerOnFloor())
        {
            state = BossState.Run;
        }
        else
        {
            state = BossState.Idle;
            StartCoroutine(IdleState());
        }
    }

    private IEnumerator StompAction()
    {
        isStomping = true;
        state = BossState.Stomp;

        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(1f);

        float distanceToFoot = Vector3.Distance(footPoint.position, player.position);
        if (distanceToFoot <= stompRange)
        {
            LightDecayStatusBar lightBar = player.GetComponentInChildren<LightDecayStatusBar>();
            if (lightBar != null)
            {
                lightBar.DamageTaken(10f);
            }
        }

        yield return new WaitForSeconds(0.5f);

        lastStompTime = Time.time;
        isStomping = false;
        state = BossState.Run;
    }

    private void IdleLogic()
    {
        if (bossHealth <= 50 && IsPlayerOnFloor() && !isStomping && !isThrowing)
        {
            state = BossState.Run;
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    private void RunState()
    {
        if (isStomping) return;

        float distanceToBoss = Vector3.Distance(transform.position, player.position);
        float distanceToFoot = Vector3.Distance(footPoint.position, player.position);

        bool shouldRun = false;

        if (distanceToBoss > closeRange)
        {
            shouldRun = true;
        }
        else
        {
            if (distanceToFoot <= stompRange && Time.time >= lastStompTime + stompCooldown)
            {
                animator.SetBool("Running", false);
                StartCoroutine(StompAction());
            }
            else if (distanceToFoot > stompRange)
            {
                shouldRun = true;
            }
            else
            {
                animator.SetBool("Running", false);
                state = BossState.Idle;
                StartCoroutine(IdleState());
            }
        }

        animator.SetBool("Running", shouldRun);
        if (shouldRun)
        {
            float actualSpeed = (distanceToBoss > closeRange) ? runSpeed : runSpeed * 0.5f;
            MoveTowards(player.position, actualSpeed);
        }
    }

    private void LookAtPlayer(float turnSpeed)
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 ylessTarget = new Vector3(target.x, transform.position.y, target.z);
        Vector3 direction = (ylessTarget - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            if (lightBar != null)
            {
                lightBar.DamageTaken(2f);
            }
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

    private void UpdateHealthBar()
    {
        if (healthBar)
            healthBar.fillAmount = bossHealth / 100f;
    }
}