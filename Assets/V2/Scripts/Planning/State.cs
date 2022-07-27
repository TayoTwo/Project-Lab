using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IComparable<State>
{

    public string stateName;
    public bool value;

    public State(string n, bool v){

        stateName = n;
        value = v;

    }

    public int CompareTo(State other){

        if(other == null){

            return 0;

        }

        if(other.stateName.Equals(stateName)){

            return 1;

        } else {

            return 0;

        }


    }
   
}
