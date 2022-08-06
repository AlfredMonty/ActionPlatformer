using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    PlayerUI playerUI;
    EnemyAnimator enemyAnimator; 
    public NavMeshAgent navMeshAgent;
    public float waitTime = 4f;
    public float rotateTime = 2f;
    public float walkSpeed = 6f;
    public float runSpeed = 9f;

    public float radius = 15f;
    public float angle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshRes = 1f;
    public int edgeIterations = 4;
    public float edgeDist = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPos = Vector3.zero;
    public Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    public bool m_PlayerInRange;
    public bool m_PlayerNear;
    public bool m_IsPatrol;
    public bool m_CaughtPlayer;

    private void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_PlayerNear = false; 

        m_WaitTime = waitTime;
        m_TimeToRotate = rotateTime;

        m_PlayerPosition = Vector3.zero;

        m_CurrentWaypointIndex = 0;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); 
    }

    void Update()
    {
        ViewEnvironment(); 

        if (!m_IsPatrol)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    public void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    private void Chase()
    {
        m_PlayerNear = false;
        playerLastPos = Vector3.zero; 

        if (!m_CaughtPlayer)
        {
            Move(runSpeed);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, player.transform.position) >= 6f)
            {
                m_PlayerInRange = false; 
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(walkSpeed);
                m_TimeToRotate = rotateTime;
                m_WaitTime = waitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patrol()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(walkSpeed);
                FindingPlayer(playerLastPos);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPos = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); 
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextWaypoint();
                    Move(walkSpeed);
                    m_WaitTime = waitTime; 
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime; 
                }
            }
        }
    }

    public void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed; 
    }
    public void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
    }
    public void NextWaypoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }


    public void FindingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player); 
        if (Vector3.Distance(transform.position, player) <= 0.3f)
        {
            if (m_WaitTime <= 0f)
            {
                m_PlayerNear = false;
                Move(walkSpeed);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = waitTime;
                m_TimeToRotate = rotateTime; 
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime; 
            }
        }
    }

    public void ViewEnvironment()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, radius, playerMask);
        
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 playerDir = (player.position - transform.position).normalized; 
            if  (Vector3.Angle(transform.forward, playerDir) < angle/2)
            {
                float distToPlayer = Vector3.Distance(transform.position, player.position); 
                if (!Physics.Raycast(transform.position, playerDir, distToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false; 
                }
                else
                {
                    m_PlayerInRange = false; 
                }
            }
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position; 
            }
        }
    }
}
