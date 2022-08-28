using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeToolAction : Action
{

    public GameObject tool;

    public MakeToolAction(){

        List<State> p = new List<State>();
        p.Add(new State("hasWood",true));
        p.Add(new State("hasOre",true));
        needsInRange = true;
        List<State> e = new List<State>();
        e.Add(new State("hasTool",true));

        actionName = "MakeTool";
        preCons = p;
        effects = e;

    }

    void UpdateClosestWorkBench(Agent agent){

        GameObject[] benches = GameObject.FindGameObjectsWithTag("Workbench");
        target = benches[0].transform;

        foreach(GameObject bench in benches){

            int disA = agent.gridManager.CalculateCost(agent.transform.position,target.position);
            int disB = agent.gridManager.CalculateCost(agent.transform.position,bench.transform.position);


            if(disB < disA){

                target = bench.transform;

            }

        }

    }

    public override int getCost(List<State> worldState,Agent agent){

        if(GameObject.FindGameObjectsWithTag("Workbench").Length > 0){

            UpdateClosestWorkBench(agent);
            return agent.gridManager.CalculateCost(agent.transform.position,target.position);

        } else {

            return 0;

        }

    }

    IEnumerator MakeTool(Agent agent){

        if(busy) yield break;

        if(target == null) {

            UpdateClosestWorkBench(agent);
            yield break;

        } 

        busy = true;

        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            agent.backpack.wood--;
            agent.backpack.ore--;
            GameObject pick = (GameObject)Instantiate(tool,transform.position,Quaternion.identity);
            agent.backpack.tool = pick;

        }

        busy = false;

    }

    public override bool isValid(){
        
        if(GameObject.FindGameObjectsWithTag("Workbench").Length == 0) return false;

        return true; 

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        StartCoroutine(MakeTool(agent));

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }
}
