using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CutTreeAction : Action
{

    public Vector3 treeLocation;

    public CutTreeAction(){

        List<State> p = new List<State>();
        p.Add(new State("inRange",true));
        List<State> e = new List<State>();
        e.Add(new State("hasWood",true));

        actionName = "CutTree";
        preCons = p;
        effects = e;

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
