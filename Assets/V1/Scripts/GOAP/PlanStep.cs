using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlanStep {
    public GOAPAction action;
    public List<GOAPState> desiredState;
    
    public PlanStep(GOAPAction a,List<GOAPState> dws){

        action = a;
        desiredState = dws;

    }

}
