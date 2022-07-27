using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public string actionName;
    public int cost;
    public string data;
    public List<State> preCons;
    public List<State> effects;

    public Action(string name, List<State> pre,List<State> eff){

        actionName = name;
        preCons = pre;
        effects = eff;

    }

    public List<State> getPrecons(){

        return preCons;

    }

    public List<State> getEffects(){

        return effects;

    }

    public int getCost(List<State> worldState,NPCController npc){

        //Depending on the action calculate the action cost differently
        string[] actionParts = actionName.Split('|');
        string prefix = actionParts[0];
        string suffix = actionParts[1];
        Debug.Log(suffix);

        switch(prefix){

            case "MoveTo":
                string[] locationParts = suffix.Split(',');
                Vector3 location = new Vector3(float.Parse(locationParts[0]), float.Parse(locationParts[1]),float.Parse(locationParts[2]));

                //Most likely doesn't work rn

                return npc.pathFinder.CalculateCost(npc.transform.position,location);;
            case "Find":

                GameObject[] targets = GameObject.FindGameObjectsWithTag(suffix);

                Vector3 closest = targets[0].transform.position;

                //Find the closest one
                foreach(GameObject s in targets){

                    float sDistance = Vector3.Distance(transform.position,s.transform.position);
                    float cDistance = Vector3.Distance(transform.position,closest);

                    if(sDistance < cDistance){

                        closest = s.transform.position;

                    }

                }

                return npc.pathFinder.CalculateCost(npc.transform.position,closest);
            case "Collect":

                //Resource amount of resource? time needed to collect? etc

                return 1;

            default:

                return cost;

        }

    }

}
