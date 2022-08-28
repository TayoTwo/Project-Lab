using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickShroomAction : Action
{

    public PickShroomAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        needsInRange = true;
        e.Add(new State("hasShroom",true));

        actionName = "PickShroom";
        preCons = p;
        effects = e;

    }

    void UpdateClosestShroom(Agent agent){

        GameObject[] shrooms = GameObject.FindGameObjectsWithTag("Shroom");
        if(shrooms.Length == 0) return;
        target = shrooms[0].transform;

        foreach(GameObject shroom in shrooms){

            int disA = agent.gridManager.CalculateCost(agent.transform.position,target.position);
            int disB = agent.gridManager.CalculateCost(agent.transform.position,shroom.transform.position);

            if(disB < disA){

                target = shroom.transform;

            }

        }

    }

    public override int getCost(List<State> worldState,Agent agent){

        if(GameObject.FindGameObjectsWithTag("Shroom").Length > 0){

            UpdateClosestShroom(agent);
            return agent.gridManager.CalculateCost(agent.transform.position,target.position);

        } else {

            return 0;

        }

    }
    
    IEnumerator PickShroom(Agent agent){
    
        if(busy) yield break;

        if(target == null) {

            UpdateClosestShroom(agent);
            yield break;

        }

        busy = true;

        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            agent.backpack.shrooms++;
            Destroy(target.root.gameObject);

        }

        busy = false;

    }

    public override bool isValid(){
        
        if(GameObject.FindGameObjectsWithTag("Shroom").Length == 0) return false;

        return true; 

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        StartCoroutine(PickShroom(agent));

        base.perform(agent);

        return allConditionsMet;

    }
}
