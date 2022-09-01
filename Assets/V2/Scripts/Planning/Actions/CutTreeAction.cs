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

    IEnumerator CutTree(Agent agent){

        //If we are alreay doing this action exit
        if(busy) yield break;

        if(target == null) {

            UpdateClosestTree(agent);
            yield break;

        } 

        busy = true;
        //Delay this action to allow animations to play
        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            agent.backpack.wood++;
            Destroy(target.root.gameObject);

        }

        busy = false;

    }

    public override int getCost(List<State> worldState,Agent agent){

        //If a tree hasn't been selected and there are trees in the scene calculate the cost to the nearest tree
        if(target == null && GameObject.FindGameObjectsWithTag("Tree").Length > 0){

            UpdateClosestTree(agent);
            return agent.gridManager.CalculateCost(agent.transform.position,target.position);

        //Calculate the cost to the selected tree
        } else if(target != null){

            return agent.gridManager.CalculateCost(agent.transform.position,target.position);

        } else {

            return 0;

        }

    }

    public override bool isValid(Agent agent){
        
        if(GameObject.FindGameObjectsWithTag("Tree").Length == 0) return false;

        return true; 

    }

    public override bool perform(Agent agent){

        StartCoroutine(CutTree(agent));

        base.perform(agent);

        return allConditionsMet;

    }

}
