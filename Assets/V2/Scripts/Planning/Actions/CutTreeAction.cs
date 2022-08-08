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

            int disA = agent.pathFinder.CalculateCost(agent.transform.position,target.position);
            int disB = agent.pathFinder.CalculateCost(agent.transform.position,tree.transform.position);


            if(disB < disA){

                target = tree.transform;

            }

        }

    }

    public override int getCost(List<State> worldState,Agent agent){

        UpdateClosestTree(agent);
        
        return agent.pathFinder.CalculateCost(agent.transform.position,target.position);

    }

    IEnumerator CutTree(Agent agent){

        //Debug.Log("CUTTING");

        UpdateClosestTree(agent);

        if(target == null) yield break;

        //Debug.Log("YUP " + agent.isWithinRange(target.position));

        if(agent.isWithinRange(target.position)){

            //Debug.Log("DESTROYING");
            Destroy(target.root.gameObject);
            agent.backpack.wood++;

        }

        yield return new WaitForSeconds(1.433f);

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        StartCoroutine(CutTree(agent));

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
