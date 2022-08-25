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
        e.Add(new State("treeChopped",true));

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

        if(target == null) {

            UpdateClosestTree(agent);
            yield break;

        }

        if(agent.isWithinRange(target.position)){

            //Debug.Log("DESTROYING");
            Destroy(target.root.gameObject,1.433f);
            agent.backpack.wood++;

        }

        yield return new WaitForSeconds(1.433f);

        agent.worldState.Find(x => x.key == "treeChopped").SetValue(true);

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

        if(allConditionsMet){

            agent.worldState.Find(x => x.key == "treeChopped").SetValue(false);

        }

        return allConditionsMet;

    }

}
