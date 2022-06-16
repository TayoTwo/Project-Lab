using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plan { 
    [SerializeField]
    public List<GOAPAction> actions;
    public int cost;

    public Plan(List<GOAPAction> a,int c){

        actions = a;
        cost = c;

    }

    public void AddToPlan(GOAPAction action,int c){

        actions.Add(action);
        cost = c;

    }

    public void AddToCost(int x){

        cost += x;

    }

    public List<GOAPAction> GetActions(){

        return actions;

    }

    public int getCost(){

        return cost;

    }

}
