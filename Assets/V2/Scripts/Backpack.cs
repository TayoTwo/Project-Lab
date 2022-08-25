using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{

    public int wood;
    public int ore;
    public GameObject tool;
    public Transform handSlot;
    Agent agent;

    void Start(){

        agent = GetComponent<Agent>();

    }

    void Update(){

        if(wood > 0){

            agent.worldState.Find(x => x.key == "hasWood").SetValue(true);

        } else {

            agent.worldState.Find(x => x.key == "hasWood").SetValue(false);

        }

        if(ore > 0){

            agent.worldState.Find(x => x.key == "hasOre").SetValue(true);

        } else {

            agent.worldState.Find(x => x.key == "hasOre").SetValue(false);

        }

        if(tool != null){

            tool.transform.parent = handSlot;
            tool.transform.localPosition = Vector3.zero;
            tool.transform.localEulerAngles = new Vector3(0,90f,0);

            if(tool.name.Contains("Pickaxe")){

                agent.worldState.Find(x => x.key == "hasPickaxe").SetValue(true);

            }

        }

    }

}