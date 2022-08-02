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

        return true;

    }
}
