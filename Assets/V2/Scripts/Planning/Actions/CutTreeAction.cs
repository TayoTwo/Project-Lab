using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CutTreeAction : Action
{

    public CutTreeAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        needsInRange = true;
        e.Add(new State("hasWood",true));

        actionName = "CutTree";
        preCons = p;
        effects = e;

    }

    void UpdateClosestTree(Agent agent){

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        if(trees.Length == 0) return;
        target = trees[0].transform;

        foreach(GameObject tree in trees){

            int disA = agent.gridManager.CalculateCost(agent.transform.position,target.position);
            int disB = agent.gridManager.CalculateCost(agent.transform.position,tree.transform.position);


            if(disB < disA){

                target = tree.transform;

            }

        }

    }

    public override int getCost(List<State> worldState,Agent agent){

        if(GameObject.FindGameObjectsWithTag("Tree").Length > 0){

            UpdateClosestTree(agent);
            return agent.gridManager.CalculateCost(agent.transform.position,target.position);

        } else {

            return 0;

        }

    }

    IEnumerator CutTree(Agent agent){

        if(busy) yield break;

        if(target == null) {

            UpdateClosestTree(agent);
            yield break;

        } 

        busy = true;

        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            //Debug.Log("DESTROYING");
            agent.backpack.wood++;
            Destroy(target.root.gameObject);

        }


        busy = false;

    }

    public override bool isValid(){
        
        if(GameObject.FindGameObjectsWithTag("Tree").Length == 0) return false;

        return true; 

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        StartCoroutine(CutTree(agent));

        base.perform(agent);

        return allConditionsMet;

    }

}
