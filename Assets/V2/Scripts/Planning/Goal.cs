using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Goal : ScriptableObject
{

    public string goalName;
    public int priority;
    [SerializeField]
    public List<State> desiredWorldState;

    public Goal (string name,List<State> ds){

        goalName = name;
        desiredWorldState = ds;

    }

    public bool isValid(Agent agent){

        bool valid = true;

        foreach(State s in desiredWorldState){

            if(agent.worldState.Find(x => x.key == s.key).value == s.value){

                valid = false;

            }

        }

        switch(goalName){

            case "Help":

                if(agent.GetComponent<GiveAction>().target == null){

                    valid = false;

                }

                break;

        }

        //Debug.Log(goalName + " is " + valid);

        return valid;

    }
}
