using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    private AiAgent agent;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float chaseSpeed;
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    float timer = 0.0f;

    bool isPlayerInSight = false; // Added flag to track player visibility

    public AiChasePlayerState(AiAgent agent, float chaseSpeed)
    {
        this.agent = agent;
        this.chaseSpeed = chaseSpeed;
        navMeshAgent = agent.GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();
    }

    public void Enter(AiAgent agent)
    {
        isPlayerInSight = false; // Reset player visibility flag on state enter

        this.agent = agent;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = chaseSpeed; // Set the speed of the NavMeshAgent
        animator.SetFloat("Speed", chaseSpeed);
    }

    public void Exit(AiAgent agent)
    {
        navMeshAgent.isStopped = true;
        animator.SetFloat("Speed", 0f);
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled || agent.playerTransform == null || agent.isDead)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = maxTime;
        }

        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            // Player is in sight, start or continue chasing
            isPlayerInSight = true;
        }

        if (isPlayerInSight)
        {
            agent.navMeshAgent.isStopped = false; // Resume chasing
        }
        else
        {
            agent.navMeshAgent.isStopped = true; // Stop chasing and transition to idle state
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
    }
}