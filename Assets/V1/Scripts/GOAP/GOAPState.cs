using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GOAPState {

    public string key;
    public bool value;

    public GOAPState (string k,bool v){

        key = k;
        v = value;

    }

    public void SetValue(bool v){

        value = v;
        
    }

    public bool GetValue(GOAPAgent agent){

        return value;

    }

}
