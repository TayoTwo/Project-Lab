using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GOAPGoal {

    public string goalName;
    public int priority;
    public GOAPWorldState<string,bool> desiredWorldState;

    public GOAPGoal (GOAPWorldState<string,bool>  ds){

        desiredWorldState = ds;

    }

    public bool isValid(GOAPWorldState<string,bool> worldState){

        switch(goalName){

            default:
                return false;

        }

    }

}
