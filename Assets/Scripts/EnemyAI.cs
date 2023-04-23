using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, Attack, Flee, Wait }
    public enum MovementMode { Wander, Patrol }

    [SerializeField] private float waitTime = 2f;
    [SerializeField] private State currentState = State.Idle;
    [SerializeField] private float chaseDistance = 10f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float fleeDistance = 20f;
    [SerializeField] private float timeBetweenAttacks = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float enemySpeed = 3.5f;
    [SerializeField] private float patrolThreshold = 2f;
    [SerializeField] private float attackStopDistance = 1.5f;

    [Header("Movement Mode")]
    [SerializeField] private MovementMode currentMovementMode = MovementMode.Patrol;
    [SerializeField] private float wanderRadius = 10f;


    private Animator animator;
    private State lastState;
    private float waitTimer;
    private NeedsSystem playerNeedsSystem;


    [SerializeField] private List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;

    private NavMeshAgent agent;
    private Transform player;
    private float attackTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Patrol;
        animator = GetComponentInChildren<Animator>();
        playerNeedsSystem = player.GetComponent<NeedsSystem>();

        agent.speed = enemySpeed;
        attackTimer = timeBetweenAttacks;
    }


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer < chaseDistance)
                {
                    currentState = State.Chase;
                }
                break;
            case State.Patrol:
                Patrol();
                if (distanceToPlayer < chaseDistance)
                {
                    currentState = State.Chase;
                }
                break;
            case State.Chase:
                Debug.Log("Entered Chase state");
                Chase();
                Debug.Log(agent.remainingDistance);
                if (distanceToPlayer <= attackDistance)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > fleeDistance)
                {
                    currentState = State.Flee;
                }
                break;
            case State.Attack:
                Attack();
                if (distanceToPlayer > attackDistance && distanceToPlayer < fleeDistance)
                {
                    currentState = State.Chase;
                }
                else if (distanceToPlayer >= fleeDistance)
                {
                    currentState = State.Flee;
                }
                break;
            case State.Flee:
                Flee();
                if (distanceToPlayer > chaseDistance)
                {
                    lastState = State.Patrol;
                    currentState = State.Wait;
                    waitTimer = 0f;
                }
                break;
            case State.Wait:
                waitTimer += Time.deltaTime;

                if (distanceToPlayer <= attackDistance)
                {
                    currentState = State.Attack;
                }
                else if (waitTimer >= waitTime)
                {
                    if (lastState == State.Patrol && patrolPoints.Count > 0) // Added a condition to check if patrolPoints.Count is greater than 0
                    {
                        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                    }
                    currentState = lastState;
                }
                break;

        }
        animator.SetFloat("moveSpeed", agent.velocity.magnitude);
    }



    void Patrol()
    {
        if (currentMovementMode == MovementMode.Patrol)
        {
            if (patrolPoints.Count == 0) return;

            agent.SetDestination(patrolPoints[currentPatrolIndex].position);

            float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position);
            if (distanceToPatrolPoint <= patrolThreshold)
            {
                lastState = currentState;
                currentState = State.Wait;
                waitTimer = 0f;
            }
        }
        else if (currentMovementMode == MovementMode.Wander)
        {
            if (agent.remainingDistance < patrolThreshold)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += transform.position;
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(randomDirection, out navMeshHit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(navMeshHit.position);
                }
            }
        }
    }


    void Chase()
    {
        // Rotate enemy to face player smoothly
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemySpeed);
        agent.SetDestination(player.position);

    }


    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // Rotate enemy to face player smoothly
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemySpeed);

        if (distanceToPlayer > attackStopDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks && distanceToPlayer <= attackDistance)
        {
            attackTimer = 0f;
            animator.SetTrigger("AttackTrigger"); // Trigger the attack animation

        }
    }


    public void ApplyDamageToPlayer()
    {
        Debug.Log("Enemy attacking!");
        playerNeedsSystem.ApplyDamage(attackDamage);
    }


    void Flee()
    {
        Vector3 directionToPlayer = transform.position - player.position;
        Vector3 fleePosition = transform.position + directionToPlayer.normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        float distanceToFleePoint = Vector3.Distance(transform.position, agent.destination);
        if (distanceToFleePoint <= patrolThreshold)
        {
            lastState = currentState;
            currentState = State.Wait;
            waitTimer = 0f;
        }
    }

}