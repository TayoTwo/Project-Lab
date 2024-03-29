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

    public bool needsRangeCheck(){

        return needsInRange;

    }

    public virtual int getCost(List<State> worldState,Agent agent){

        return cost;

    }

    public virtual bool isValid(Agent agent){

        return true;

    }

    public virtual bool perform(Agent agent){

        allConditionsMet = true;

        //Check if the effects have done what they state they do
        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
