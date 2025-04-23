using UnityEngine;
using UnityEngine.AI;

public class GoblinScript : MonoBehaviour
{
    public GoblinStates currentState;

    public float visionRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 10f;
    public float idleTime = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private float idleTimer;
    private Vector3 patrolTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        TransitionToState(GoblinStates.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case GoblinStates.Idle: HandleIdle(); break;
            case GoblinStates.Walk: HandleWalk(); break;
            case GoblinStates.Chase: HandleChase(); break;
            case GoblinStates.Attack: HandleAttack(); break;
            case GoblinStates.Dead: break;
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
        if (agent.remainingDistance < 0.5f)
        {
            TransitionToState(GoblinStates.Idle);
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(-agent.velocity.normalized);
        }

        if (PlayerInVision())
            TransitionToState(GoblinStates.Chase);
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
        }
    }

    void HandleChase()
    {
        if (player == null) return;

        agent.SetDestination(player.position);

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(-agent.velocity.normalized);
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TransitionToState(GoblinStates.Attack);
        }
        else if (distance > visionRange * 1.5f)
        {
            TransitionToState(GoblinStates.Idle);
        }
    }

    void HandleAttack()
    {
        agent.SetDestination(transform.position); // зупинити

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(-agent.velocity.normalized);
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            TransitionToState(GoblinStates.Chase);
        }
    }

    public void Die()
    {
        agent.enabled = false;
        TransitionToState(GoblinStates.Dead);
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f); // щоб не валялись вічно
    }

    bool PlayerInVision()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > visionRange) return false;

        if (agent.velocity.sqrMagnitude <= 0.01f) return false; // якщо не рухається — нікого не бачить

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
        currentState = newState;

        // Зміна анімацій відповідно до стану
        animator.SetBool("isWalking", newState == GoblinStates.Walk || newState == GoblinStates.Chase);
        if (newState == GoblinStates.Idle) animator.Play("01_Idle");
        if (newState == GoblinStates.Attack) animator.SetTrigger("triggerAttack"); ;
        if (newState == GoblinStates.Dead) animator.SetTrigger("triggerDeath"); ;
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