using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleCarController : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float maxSteerAngle = 30f;
    public float maxMotorTorque = 200f; // Adjust this value based on your car's specs
    [SerializeField] float maxSpeed = 10f;

    public NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GoToNextWaypoint();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    public float CurrentSteer()
    {
        if (agent.remainingDistance <= 0.5f)
        {
            return 0f;
        }

        Vector3 directionToTarget = (agent.steeringTarget - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        float steer = Mathf.Clamp(angleToTarget / maxSteerAngle, -1f, 1f);

        return steer;
    }

    public float CurrentMotorTorque()
    {
        float agentDesiredSpeed = agent.desiredVelocity.magnitude;
        float speedRatio = Mathf.Clamp01(agentDesiredSpeed / maxSpeed);
        float motorTorque = speedRatio * maxMotorTorque;
        return motorTorque;
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}
