using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiPatrolState : AiState
{
    private AiAgentConfig config;
    private int currentPatrolIndex;
    private NavMeshAgent navMeshAgent;
    private Transform[] patrolPoints;
    private bool isRandomizing;
    private float nextPatrolTime;
    private float initialStoppingDistance;

    public AiPatrolState(AiAgentConfig config)
    {
        this.config = config;
    }

    public void Enter(AiAgent agent)
    {
        currentPatrolIndex = 0;
        navMeshAgent = agent.GetComponent<NavMeshAgent>();
        patrolPoints = agent.patrolPoints;
        isRandomizing = false;

        // Save the initial stopping distance and set a small stopping distance for patrol.
        initialStoppingDistance = navMeshAgent.stoppingDistance;
        navMeshAgent.stoppingDistance = 0.1f;

        // Set patrol speed within the min and max values specified in the config.
        navMeshAgent.speed = Random.Range(config.minWalkSpeed, config.maxWalkSpeed);

        SetNextPatrolPoint();
    }

    public void Exit(AiAgent agent)
    {
        // Restore the initial stopping distance when exiting the patrol state.
        navMeshAgent.stoppingDistance = initialStoppingDistance;

        // Stop the agent when exiting the patrol state.
        navMeshAgent.isStopped = true;
    }

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Update(AiAgent agent)
    {
        if (navMeshAgent.velocity.magnitude < 0.1f && !isRandomizing)
        {
            // If the agent has stopped moving, start randomizing the next patrol point.
            isRandomizing = true;
            nextPatrolTime = Time.time + Random.Range(config.minPatrolDelay, config.maxPatrolDelay);
        }

        if (isRandomizing && Time.time >= nextPatrolTime)
        {
            // Randomize the next patrol point and start moving again.
            isRandomizing = false;
            SetNextPatrolPoint();
        }
    }

    private void SetNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        // Set the next destination for the agent to patrol.
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

        // Resume movement after setting the destination.
        navMeshAgent.isStopped = false;
    }

    private IEnumerator RandomizePatrol()
    {
        // Wait for a short delay before randomizing the next patrol point.
        yield return new WaitForSeconds(Random.Range(config.minPatrolDelay, config.maxPatrolDelay));

        if (patrolPoints.Length <= 1)
        {
            // If there is only one patrol point, no need to randomize.
            isRandomizing = false;
            SetNextPatrolPoint();
            yield break;
        }

        // Choose a random index for the next patrol point.
        int nextIndex = currentPatrolIndex;
        while (nextIndex == currentPatrolIndex)
        {
            nextIndex = Random.Range(0, patrolPoints.Length);
        }

        // Set the new patrol destination.
        navMeshAgent.SetDestination(patrolPoints[nextIndex].position);
        currentPatrolIndex = nextIndex;

        isRandomizing = false;
    }

    // Function to shuffle the patrol points list
    private void ShufflePatrolPoints(AiAgent agent)
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            int randomIndex = Random.Range(i, patrolPoints.Length);
            Transform temp = patrolPoints[i];
            patrolPoints[i] = patrolPoints[randomIndex];
            patrolPoints[randomIndex] = temp;
        }
        // Update the shuffled patrol points in the AiAgent
        agent.patrolPoints = patrolPoints;
    }
}