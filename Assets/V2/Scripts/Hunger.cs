using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{

    public float hunger = 1;
    public float starveRate = 0.1f;

    Agent agent;

    void Awake(){

        agent = GetComponent<Agent>();

    }

    void FixedUpdate(){

        DepeleteHunger();

    }

    public void DepeleteHunger(){

        hunger -= starveRate * Time.fixedDeltaTime;

        hunger = Mathf.Clamp(hunger,0,1);

        bool isHungry = false;

        isHungry = (hunger == 0) ? true : false;
        
        agent.worldState.Find(x => x.key == "isHungry").SetValue(isHungry);

        if(isHungry){

            agent.ChangeGoal("EatFood");

        }

    }

}
