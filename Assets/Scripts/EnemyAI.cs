using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, Attack, Flee, Wait }

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

    private Animator animator;
    private State lastState;
    private float waitTimer;

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
                Chase();
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
                if (distanceToPlayer > attackDistance)
                {
                    currentState = State.Chase;
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
                    if (lastState == State.Patrol)
                    {
                        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                    }
                    currentState = lastState;
                }
                break;
        }
        animator.SetFloat("moveSpeed", agent.velocity.magnitude);

        // Update the isAttacking parameter based on the current state
        animator.SetBool("isAttacking", currentState == State.Attack);


    }



    void Patrol()
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


    void Chase()
    {
        agent.SetDestination(player.position);
        agent.isStopped = false;
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackStopDistance)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
            animator.SetBool("isAttacking", false);
        }
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks && distanceToPlayer <= attackDistance)
        {
            attackTimer = 0f;
            animator.SetBool("isAttacking", true);

            // Implement logic to deal damage to the player
            Debug.Log("Enemy attacking!");
        }
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
