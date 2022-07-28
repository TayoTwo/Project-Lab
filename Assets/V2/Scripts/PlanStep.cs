using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlanStep {
    public Action action;
    public List<State> desiredState;
    
    public PlanStep(Action a,List<State> dws){

        action = a;
        desiredState = dws;

    }

}
