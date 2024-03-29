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

            case "GetSupport":

                if(agentController.AgentCount() > 1){

                    return !agentController.health.isHealthy;

                }

                return false;

            case "Respond":

                return agentController.hasBeenCalled;

            default:
                return false;

        }

    }

}
