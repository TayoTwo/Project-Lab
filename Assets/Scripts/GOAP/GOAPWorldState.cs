using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GOAPWorldState<T,W> {

    public Dictionary<T,W> data;

    public GOAPWorldState (T key,W value){

        data.Clear();
        data.Add(key,value);

    }

    public void SetValue(T key,W value){

        data[key] = value;
        
    }

    public W GetValue(T key){

        return data[key];

    }

    public bool HasKey(T key){

        return data.ContainsKey(key);

    }

    public int GetLength(){

        return data.Count;

    }

}
