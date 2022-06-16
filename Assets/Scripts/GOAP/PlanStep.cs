using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlanStep {
    private GOAPAction action;
    private GOAPWorldState<string,bool> desiredState;
    
    public PlanStep(GOAPAction a,GOAPWorldState<string,bool> dws){

        action = a;
        desiredState = dws;

    }

    public GOAPAction GetAction(){

        return action;

    }

    public GOAPWorldState<string,bool> getDesiredWorldState(){

        return desiredState;

    }

}
