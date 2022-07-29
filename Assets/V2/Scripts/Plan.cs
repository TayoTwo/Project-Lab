using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plan { 
    [SerializeField]
    public List<Action> actions;
    public int cost;

    public Plan(){

        
    }

    public Plan(List<Action> a,int c){

        actions = a;
        cost = c;

    }

    public void AddToPlan(Action action,int c){

        actions.Add(action);
        cost += c;

    }

    public void AddToCost(int x){

        cost += x;

    }

}
