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

                switch(state.key){

                    case "hasWood":
                        if(agent.backpack.wood > 0){
                            agent.backpack.wood--;
                            reciever.wood++;
                            agent.worldState.Find(x => x.key == "hasHelped").SetValue(true);
                        }
                        break;

                    case "hasOre":
                        if(agent.backpack.ore > 0){
                            agent.backpack.ore--;
                            reciever.ore++;
                            agent.worldState.Find(x => x.key == "hasHelped").SetValue(true);
                        }
                        break;

                    case "hasShrooms":
                        if(agent.backpack.shrooms > 0){
                            agent.backpack.shrooms--;
                            reciever.shrooms++;
                            agent.worldState.Find(x => x.key == "hasHelped").SetValue(true);
                        }
                        break;

                }

            }

        }
        
        busy = false;

    }

    public override bool isValid(Agent agent){

        if(target == null || target.GetComponent<Agent>().agentState != AgentState.SIGNAL){

            target = null;
            return false;

        } 

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
            agent.worldState.Find(x => x.key == "hasHelped").SetValue(false);
            busy = false;

        }

        return allConditionsMet;

    }

}
