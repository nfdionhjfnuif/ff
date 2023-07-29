using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public List<AiAgent> agents = new List<AiAgent>();

    void Update()
    {
        foreach (AiAgent agent in agents)
        {
            agent.stateMachine.update();
        }
    }
}

