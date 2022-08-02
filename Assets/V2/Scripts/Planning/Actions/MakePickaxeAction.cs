using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePickaxeAction : Action
{

    public GameObject pickaxe;

    public MakePickaxeAction(){

        List<State> p = new List<State>();
        p.Add(new State("hasWood",true));
        p.Add(new State("hasOre",true));
        List<State> e = new List<State>();
        e.Add(new State("haxPickaxe",true));

        actionName = "MakePickaxe";
        preCons = p;
        effects = e;

    }

    void UpdateClosestWorkBench(Agent agent){

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Workbench");
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

        UpdateClosestWorkBench(agent);
        
        return agent.pathFinder.CalculateCost(agent.transform.position,target.position);

    }

    IEnumerator MakePickaxe(Agent agent){

        if(agent.isWithinRange(target.position)){

            agent.backpack.wood--;
            agent.backpack.ore--;
            GameObject pick = (GameObject)Instantiate(pickaxe,transform.position,Quaternion.identity);
            agent.backpack.tool = pick;

        }

        yield return new WaitForSeconds(1.433f);

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        StartCoroutine(MakePickaxe(agent));

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }
}
