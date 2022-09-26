using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State : IComparable<State>
{

    public string key;
    public bool value;

    public State(string k, bool v){

        key = k;
        value = v;

    }

    public void SetValue(bool v){

        value = v;
        
    }

    public bool GetValue(Agent agent){

        return value;

    }

    public int CompareTo(State other){

        if(other == null){

            return 0;

        }

        if(other.key.Equals(key)){

            return 1;

        } else {

            return 0;

        }


    }
   
}
