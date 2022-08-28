using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAction : Action
{

    public GiveAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        e.Add(new State("hasHelped",true));
        needsInRange = true;
        actionName = "Give";
        preCons = p;
        effects = e;

    }

    IEnumerator Give(Agent agent){

        Backpack reciever = target.GetComponent<Backpack>();

        if(busy || target == null) yield break;

        busy = true;

        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            foreach(State state in preCons){

                if(state.key == "hasWood"  && agent.backpack.wood > 0){

                    agent.backpack.wood--;
                    reciever.wood++;
                    agent.worldState.Find(x => x.key == "hasHelped").SetValue(true);

                }

                if(state.key == "hasOre" && agent.backpack.ore > 0){

                    agent.backpack.ore--;
                    reciever.ore++;
                    agent.worldState.Find(x => x.key == "hasHelped").SetValue(true);

                }

            }

        }
        
        busy = false;

    }

    public override bool isValid(){

        if(target == null) return false;

        return true;

    }

    public override bool perform(Agent agent){

        //Perform action
        //Find nearest tree
        //Set tree as target
        //Destroy tree
        //Add resource to inventory

        if(target != null){

            StartCoroutine(Give(agent));
            
        }

        base.perform(agent);    

        if(allConditionsMet){

            target = null;
            preCons.Clear();

        }

        return allConditionsMet;

    }

}
