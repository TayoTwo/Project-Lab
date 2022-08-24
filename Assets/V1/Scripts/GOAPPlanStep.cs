using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GOAPPlanStep {
    public GOAPAction action;
    public List<GOAPState> desiredState;
    
    public GOAPPlanStep(GOAPAction a,List<GOAPState> dws){

        action = a;
        desiredState = dws;

    }

}
