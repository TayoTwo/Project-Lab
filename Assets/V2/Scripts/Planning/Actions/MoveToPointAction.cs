using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPointAction : Action
{

    public Vector3 moveToPos;
    
    public MoveToPointAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        needsInRange = true;
        e.Add(new State("hasMovedTo",true));

        actionName = "MoveToPoint";
        preCons = p;
        effects = e;

    }


    public override int getCost(List<State> worldState,Agent agent){
        
        return agent.gridManager.CalculateCost(agent.transform.position,moveToPos);

    }

    public override bool perform(Agent agent){

        if(agent.isWithinRange(target.position)){

            agent.worldState.Find(x => x.key == "hasMovedTo").SetValue(true);
            Destroy(target.gameObject);

        }

        base.perform(agent);

        //Reset this state since it has been completed
        if(allConditionsMet){

            agent.worldState.Find(x => x.key == "hasMovedTo").SetValue(false);
            busy = false;
            
        }

        return allConditionsMet;

    }

}
