using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GOAPGoal {

    public string goalName;
    public int priority;
    public List<GOAPState> desiredWorldState = new List<GOAPState>();

    public GOAPGoal (List<GOAPState> ds){

        desiredWorldState = ds;

    }

    public bool isValid(GOAPAgent agent){

        AgentController agentController = agent.GetComponent<AgentController>();

        switch(goalName){

            case "KillPlayer":

                return !agent.worldState.Find(x => x.key == "isPlayerDead").GetValue(agent);

            case "StayHealthy":

                return !agentController.health.isHealthy;

            default:
                return false;

        }

    }

}
