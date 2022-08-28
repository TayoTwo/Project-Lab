using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [Serializable]
public class Action : MonoBehaviour
{
    public string actionName;
    public int cost;
    public Transform target;
    public bool needsInRange;
    public List<State> preCons;
    public List<State> effects;
    public bool busy;
    public bool allConditionsMet = true;

    public List<State> getPrecons(){

        return preCons;

    }

    public List<State> getEffects(){

        return effects;

    }

    public virtual int getCost(List<State> worldState,Agent npc){

        return cost;

    }

    public bool needsRangeCheck(){

        return needsInRange;

    }

    public virtual bool isValid(){

        return true;

    }

    public virtual bool perform(Agent agent){

        //Perform action

        allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
