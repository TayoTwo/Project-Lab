using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GOAPAction
{

    public string actionName;
    public int cost;
    public GOAPWorldState<string,bool> preConditions;
    public GOAPWorldState<string,bool> effects;

    public GOAPAction(string name, GOAPWorldState<string,bool> pre,GOAPWorldState<string,bool> eff){

        actionName = name;
        preConditions = pre;
        effects = eff;

    }


    public GOAPWorldState<string,bool> getPreConditions(){

        return preConditions;

    }

    public GOAPWorldState<string,bool> getEffects(){

        return effects;

    }

    
    public int getCost(GOAPWorldState<string,bool> worldState){

        return cost;

    }

    public bool isValid(GOAPAgent agent){

        bool isValid = true;

        foreach(KeyValuePair<string, bool> entry in getPreConditions().data){

            if(!agent.worldState.HasKey(entry.Key) || agent.worldState.GetValue(entry.Key) != entry.Value){

                isValid = false;

            }

        }

        return isValid;
        
    }

    public bool perform(GOAPAgent agent){

        AgentController agentController = agent.GetComponent<AgentController>();

        switch(actionName){



        }

        bool allConditionsMet = true;

        foreach(KeyValuePair<string, bool> entry in getEffects().data){

            if(!agent.worldState.HasKey(entry.Key) || agent.worldState.GetValue(entry.Key) != entry.Value){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
