using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float dieForce = 10.0f;
    public float maxSightDitance = 5.0f;
    public float minWalkSpeed = 1.0f;
    public float maxWalkSpeed = 2.0f;
    public float chaseSpeed = 6.0f;
    public float minPatrolDelay = 2.0f;
    public float maxPatrolDelay = 5.0f; 
}
