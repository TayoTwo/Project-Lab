using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAction : Action
{

    public GiveAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        e.Add(new State("hasHelped",true));

        actionName = "Give";
        preCons = p;
        effects = e;

    }

    IEnumerator Give(Agent agent){

        Backpack reciever = target.GetComponent<Backpack>();

        if(agent.isWithinRange(target.position)){

            foreach(State state in preCons){

                if(state.key == "hasWood"){

                    agent.backpack.wood--;
                    reciever.wood++;

                }

                if(state.key == "hasOre"){

                    agent.backpack.ore--;
                    reciever.ore++;

                }

            }

        }

        yield return new WaitForSeconds(1.433f);

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

        bool allConditionsMet = true;

        foreach(State state in getEffects()){

            if(!agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                allConditionsMet = false;

            }

        }

        return allConditionsMet;

    }

}
