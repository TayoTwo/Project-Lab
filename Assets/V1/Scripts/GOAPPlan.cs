using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GOAPPlan { 
    [SerializeField]
    public List<GOAPAction> actions;
    public int cost;

    public GOAPPlan(){

        
    }

    public GOAPPlan(List<GOAPAction> a,int c){

        actions = a;
        cost = c;

    }

    public void AddToPlan(GOAPAction action,int c){

        actions.Add(action);
        cost += action.cost;

    }

    public void AddToCost(int x){

        cost += x;

    }

}
