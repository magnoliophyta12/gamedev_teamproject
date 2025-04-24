using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinScript : MonoBehaviour
{
    public GoblinStates currentState;

    public float visionRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 10f;
    public float idleTime = 2f;

    public float attackCooldown = 4f;
    private float lastAttackTime = 0f;

    public int maxHealth = 2;
    private int currentHealth;
    private bool isDead = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private float idleTimer;
    private Vector3 patrolTarget;

    private HealthBarScript playerHealthbar;

    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        TransitionToState(GoblinStates.Idle);

        playerHealthbar = GameObject.Find("HealthBar").GetComponent<HealthBarScript>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case GoblinStates.Idle: HandleIdle(); break;
            case GoblinStates.Walk: HandleWalk(); break;
            case GoblinStates.Chase: HandleChase(); break;
            case GoblinStates.Attack: HandleAttack(); break;
            case GoblinStates.Dead: break;
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(-agent.velocity.normalized);
        }
    }

    void HandleIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;
            ChoosePatrolPoint();
            TransitionToState(GoblinStates.Walk);
        }

        if (PlayerInVision())
            TransitionToState(GoblinStates.Chase);
    }

    void HandleWalk()
    {
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath(); // Скидаємо шлях
            animator.SetBool("isWalking", false);
            TransitionToState(GoblinStates.Idle);
            return;
        }

        if (PlayerInVision())
        {
            agent.ResetPath(); // Зупиняємо патрулювання перед погонею
            TransitionToState(GoblinStates.Chase);
        }
    }

    void ChoosePatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
            return;
        }
        TransitionToState(GoblinStates.Idle);
    }

    void HandleChase()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TransitionToState(GoblinStates.Attack);
            return;
        }
        if (distance > visionRange * 1.5f)
        {
            TransitionToState(GoblinStates.Idle);
            return;
        }
        agent.SetDestination(player.position);
    }

    void HandleAttack()
    {
        agent.SetDestination(transform.position); // зупинити

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            TransitionToState(GoblinStates.Chase);
            return;
        }
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("triggerAttack");
        }
    }

    public void DealDamage()
    {
        playerHealthbar.ReduceHealth();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Goblin HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            TransitionToState(GoblinStates.Dead);
        }
    }

    //void HandleDie()
    //{
    //    if (isDead) return;
    //    Debug.Log("Goblin died!");
    //    //isDead = true;
    //    agent.enabled = false;
    //    GetComponent<Collider>().enabled = false;
    //    Destroy(gameObject, 1.5f); // щоб не валялись вічно
    //}

    bool PlayerInVision()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > visionRange) return false;

        Vector3 lookDirection = -agent.velocity.normalized; // куди "дивиться"
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(lookDirection, dirToPlayer);

        if (angle < 90f) // 180° поле зору
        {
            return true;
        }

        return false;
    }

    void TransitionToState(GoblinStates newState)
    {
        if (isDead && newState != GoblinStates.Dead) return;
        currentState = newState;

        switch (newState)
        {
            case GoblinStates.Attack:
                animator.SetTrigger("triggerAttack");
                break;
            case GoblinStates.Dead:
                //isDead = true;
                animator.SetTrigger("triggerDeath");
                //agent.ResetPath();
                agent.enabled = false;
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 1.2f);
                break;
            default:
                animator.SetBool("isWalking", newState == GoblinStates.Walk || newState == GoblinStates.Chase);
                break;
        }
    }
}

public enum GoblinStates
{
    Idle,
    Walk,
    Chase,
    Attack,
    Dead
}