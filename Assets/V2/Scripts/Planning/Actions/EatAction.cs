using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : Action
{
    public EatAction(){

        List<State> p = new List<State>();
        p.Add(new State("hasShrooms",true));
        List<State> e = new List<State>();
        e.Add(new State("isHungry",false));

        actionName = "Eat";
        preCons = p;
        effects = e;

    }

    public override int getCost(List<State> worldState,Agent agent){

        return 0;

    }

    IEnumerator Eat(Agent agent){

        if(busy) yield break;

        busy = true;

        yield return new WaitForSeconds(1.433f);

        agent.backpack.shrooms--;
        agent.hunger.hunger = 1;

        busy = false;

    }

    public override bool isValid(Agent agent){

        if(agent.worldState.Find(x => x.key == "isHungry").value) return true;

        return false;

    }

    public override bool perform(Agent agent){

        StartCoroutine(Eat(agent));

        base.perform(agent);

        return allConditionsMet;

    }

}
