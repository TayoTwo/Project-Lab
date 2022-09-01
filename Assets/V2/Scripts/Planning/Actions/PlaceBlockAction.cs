using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockAction : Action
{
    public int blockType = -1;

    public List<GameObject> blocks = new List<GameObject>();

    public PlaceBlockAction(){

        List<State> p = new List<State>();
        p.Add(new State("hasTool",true));
        needsInRange = true;

        List<State> e = new List<State>();
        e.Add(new State("hasPlacedBlock",true));

        actionName = "PlaceBlock";
        preCons = p;
        effects = e;

    }

    public override bool isValid(Agent agent)
    {
            switch(blockType){

                case 0:

                    return agent.worldState.Find(x => x.key == "hasWood").value;

                case 1:

                    return agent.worldState.Find(x => x.key == "hasOre").value;
                
                default:

                    return false;

            }
    }

    public override int getCost(List<State> worldState, Agent agent)
    {

        if(target == null) return 0;

        return agent.gridManager.CalculateCost(agent.transform.position,target.position);
    }

    IEnumerator PlaceBlock(Agent agent){

        if(busy || target == null) yield break;

        busy = true;

        yield return new WaitForSeconds(1.433f);

        if(agent.isWithinRange(target.position)){

            switch(blockType){

                case 0:

                    agent.backpack.wood--;
                    break;

                case 1:

                    agent.backpack.ore--;
                    break;
                default:

                    Debug.Log("INVALID BLOCK INDEX");
                    yield break;

            }

            Instantiate(blocks[blockType],target.transform.position,Quaternion.identity);

            agent.worldState.Find(x => x.key == "hasPlacedBlock").SetValue(true);

        }

        busy = false;

    }

    public override bool perform(Agent agent){

        StartCoroutine(PlaceBlock(agent));

        base.perform(agent);

        if(allConditionsMet){

            preCons.Clear();
            preCons.Add(new State("hasTool",true));
            blockType = -1;
            agent.worldState.Find(x => x.key == "hasPlacedBlock").SetValue(false);
            busy = false;

        }

        return allConditionsMet;

    }

    void OnDrawGizmos(){

        if(target != null){

            Gizmos.color = Color.yellow * 0.5f;

            Gizmos.DrawCube(target.position,Vector3.one);

        }

    }

}
