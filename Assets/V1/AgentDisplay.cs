using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentDisplay : MonoBehaviour
{

    GOAPAgent agent;
    public TMP_Text displayText;

    // Start is called before the first frame update
    void Start(){

        agent = GetComponent<GOAPAgent>();

    }

    // Update is called once per frame
    void Update(){

        displayText.text = "Goal: " + agent.currentGoal.goalName + '\n' + "Action: " + agent.currentAction.actionName;

    }  

}
