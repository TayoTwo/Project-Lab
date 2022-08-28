using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : Action
{
    public EatAction(){

        List<State> p = new List<State>();
        p.Add(new State("hasShroom",true));
        List<State> e = new List<State>();
        e.Add(new State("isHungry",false));

        actionName = "Eat";
        preCons = p;
        effects = e;

    }

    public override int getCost(List<State> worldState,Agent agent){

        return 0;

    }

    public override bool perform(Agent agent){

        agent.backpack.shrooms--;
        agent.hunger.hunger = 1;

        base.perform(agent);

        return allConditionsMet;

    }
}
