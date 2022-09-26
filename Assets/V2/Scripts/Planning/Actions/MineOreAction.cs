using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOreAction : Action
{

    public MineOreAction(){

        List<State> p = new List<State>();
        List<State> e = new List<State>();
        needsInRange = true;
        e.Add(new State("hasOre",true));

        actionName = "MineOre";
        preCons = p;
        effects = e;

    }

    void UpdateClosestOre(Agent agent){

        GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
        if(ores.Length == 0) return;
        target = ores[0].transform;

        foreach(GameObject ore in ores){

            int disA = agent.gridManager.CalculateCost(agent.transform.position,target.position);
            int disB = agent.gridManager.CalculateCost(agent.transform.position,ore.transform.position);

            if(disB < disA){

                target = ore.transform;

            }

        }

    }


    
    IEnumerator MineOre(Agent agent){
    
        if(busy) yield break;

        if(target == null) {

            UpdateClosestOre(agent);
            yield break;

        }

        busy = true;

        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            agent.backpack.ore++;
            Destroy(target.root.gameObject);

        }

        busy = false;

    }

    public override bool isValid(Agent agent){
        
        if(GameObject.FindGameObjectsWithTag("Ore").Length == 0) return false;

        return true; 

    }

    public override int getCost(List<State> worldState,Agent agent){

        if(GameObject.FindGameObjectsWithTag("Ore").Length > 0){

            UpdateClosestOre(agent);
            return agent.gridManager.CalculateCost(agent.transform.position,target.position);

        } else {

            return 0;

        }

    }

    public override bool perform(Agent agent){

        StartCoroutine(MineOre(agent));

        base.perform(agent);

        return allConditionsMet;

    }
    
}
