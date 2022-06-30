using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GOAPAction
{

    public string actionName;
    public int cost;
    public List<GOAPState> preConditions = new List<GOAPState>();
    public List<GOAPState> effects = new List<GOAPState>();

    public GOAPAction(string name, List<GOAPState> pre,List<GOAPState> eff){

        actionName = name;
        preConditions = pre;
        effects = eff;

    }


    public List<GOAPState> getPreConditions(){

        return preConditions;

    }

    public List<GOAPState> getEffects(){

        return effects;

    }

    
    public int getCost(List<GOAPState> worldState){

        return cost;

    }

    public bool isValid(GOAPAgent agent){

        bool isValid = true;

        foreach(GOAPState state in getPreConditions()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                isValid = false;

            }

        }

        return isValid;
        
    }

    public bool perform(GOAPAgent agent){

        AgentController agentController = agent.GetComponent<AgentController>();

        switch(actionName){

            case "Patrol":
                agentController.Patrol();
                break;
            case "Chase":
                agentController.Chase();
                break;
            case "Attack":
                agentController.Attack();
                break;
            case "Heal":
                agentController.FindHealthStation();
                break;


        }

        bool allConditionsMet = true;

        foreach(GOAPState state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
