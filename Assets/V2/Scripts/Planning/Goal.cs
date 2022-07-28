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

    public Goal (List<State> ds){

        desiredWorldState = ds;

    }

    public bool isValid(Agent agent){

        return true;

    }
}
