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

        GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
        if(ores.Length == 0) return;
        target = ores[0].transform;

        foreach(GameObject ore in ores){

            int disA = agent.pathFinder.CalculateCost(agent.transform.position,target.position);
            int disB = agent.pathFinder.CalculateCost(agent.transform.position,ore.transform.position);

            if(disB > disA){

                target = ore.transform;

            }

        }

    }

    public override int getCost(List<State> worldState,Agent agent){

        UpdateClosestOre(agent);
        
        return agent.pathFinder.CalculateCost(agent.transform.position,target.position);

    }
    IEnumerator MineOre(Agent agent){
        
        UpdateClosestOre(agent);

        if(target == null) yield break;

        if(agent.isWithinRange(target.position)){

            Destroy(target.root.gameObject);
            agent.backpack.ore++;

        }

        yield return new WaitForSeconds(1.433f);

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        StartCoroutine(MineOre(agent));

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }
}
