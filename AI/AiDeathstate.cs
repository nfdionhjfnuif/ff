using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathstate : AiState
{

    public Vector3 direction;

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForces(direction * agent.config.dieForce);
    }

    public void Exit(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {

    }
}
