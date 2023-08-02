using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    private float currentHealth;
    AiAgent agent;

    private void Start()
    {
        agent = GetComponent<AiAgent>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.target = this;

        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die(direction);
        }
    }

    private void Die(Vector3 direction)
    {
        AiDeathstate deathstate = agent.stateMachine.GetState(AiStateId.Death) as AiDeathstate;
        deathstate.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
    }
}