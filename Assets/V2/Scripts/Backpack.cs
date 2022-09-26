using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{

    public int wood;
    public int ore;
    public int shrooms;
    public GameObject tool;
    public Transform handSlot;
    Agent agent;

    void Start(){

        agent = GetComponent<Agent>();

    }

    void Update(){

        wood = Mathf.Clamp(wood,0,1);
        ore = Mathf.Clamp(ore,0,1);
        shrooms = Mathf.Clamp(shrooms,0,1);

        //Update the hasWood state
        if(wood > 0){

            agent.worldState.Find(x => x.key == "hasWood").SetValue(true);

        } else {

            agent.worldState.Find(x => x.key == "hasWood").SetValue(false);

        }

        //Update the hasOre state
        if(ore > 0){

            agent.worldState.Find(x => x.key == "hasOre").SetValue(true);

        } else {

            agent.worldState.Find(x => x.key == "hasOre").SetValue(false);

        }
        //Update the hasShrooms state
        if(shrooms > 0){

            agent.worldState.Find(x => x.key == "hasShrooms").SetValue(true);

        } else {

            agent.worldState.Find(x => x.key == "hasShrooms").SetValue(false);

        }

        //Update the hasTool state and move the tool to the correct position on the model
        if(tool != null){

            tool.transform.parent = handSlot;
            tool.transform.localPosition = Vector3.zero;
            tool.transform.localEulerAngles = new Vector3(0,90f,0);

            if(tool.name.Contains("Tool")){

                agent.worldState.Find(x => x.key == "hasTool").SetValue(true);

            }

        }

    }

}
