using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOreAction : Action
{

    public MineOreAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        e.Add(new State("hasOre",true));

        actionName = "MineOre";
        preCons = p;
        effects = e;

    }

    void UpdateClosestOre(Agent agent){

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Ore");
        target = trees[0].transform;

        foreach(GameObject tree in trees){

            int disA = agent.pathFinder.CalculateCost(agent.transform.position,target.position);
            int disB = agent.pathFinder.CalculateCost(agent.transform.position,tree.transform.position);


            if(disB > disA){

                target = tree.transform;

            }

        }

    }

    public override int getCost(List<State> worldState,Agent agent){

        UpdateClosestOre(agent);
        
        return agent.pathFinder.CalculateCost(agent.transform.position,target.position);

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        if(agent.isWithinRange(target.position)){

            Destroy(target.gameObject);
            agent.worldState.Find(x => x.key == "hasOre").SetValue(true);

        }

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }
}
