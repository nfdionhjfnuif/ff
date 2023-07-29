using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public AiSensor sensor;
    public Transform playerTransform;
    public float chaseSpeed;
    public List<AiAgent> otherAgents = new List<AiAgent>();
    public Transform[] patrolPoints;
    public bool isDead = false;

    private void Awake()
    {
        stateMachine = new AiStateMachine(this);
    }

    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine.RegisterState(new AiChasePlayerState(this, chaseSpeed));
        stateMachine.RegisterState(new AiDeathstate());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiPatrolState(config));
        ShufflePatrolPoints();

        stateMachine.ChangeState(initialState);

        FindOtherAgents();
    }

    void Update()
    {

    }

    private void FindOtherAgents()
    {
        otherAgents.Clear();

        AiAgent[] agents = FindObjectsOfType<AiAgent>();
        foreach (AiAgent agent in agents)
        {
            if (agent != this)
            {
                otherAgents.Add(agent);
            }
        }
    }

    public List<AiAgent> GetOtherAgents()
    {
        return otherAgents;
    }

    public void Die()
    {
        isDead = true;
        navMeshAgent.isStopped = true;
    }

    // Function to shuffle the patrol points list
    private void ShufflePatrolPoints()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            int randomIndex = Random.Range(i, patrolPoints.Length);
            Transform temp = patrolPoints[i];
            patrolPoints[i] = patrolPoints[randomIndex];
            patrolPoints[randomIndex] = temp;
        }
    }
}