using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TMP_Text agentName;
    public TMP_Dropdown currentState;
    public TMP_Text currentGoal;
    public TMP_Text currentAction;
    public TMP_Dropdown actionDropdown;
    public SelectionTool selectionTool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        UpdateUI();
    }
    
    void UpdateUI(){

        if(selectionTool.selectedObject == null || selectionTool.selectedObject.GetComponent<Agent>() == null) return;

        Agent selectedAgent = selectionTool.selectedObject.GetComponent<Agent>();

        agentName.text = selectedAgent.gameObject.name;

        if(selectedAgent.currentGoal == null || selectedAgent.currentAction == null){

            currentGoal.text = "Current Goal: None";
            currentAction.text = "Current Action: None";

        } else {

            currentGoal.text = "Current Goal: " + selectedAgent.currentGoal.goalName;
            currentAction.text = "Current Action: " + selectedAgent.currentAction.actionName;

        }

        //Availible Actions

        actionDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();

        foreach(Action a in selectedAgent.actions){

            optionData.Add(new TMP_Dropdown.OptionData(a.actionName));

        }

        //Current 
        currentState.value = (int)selectedAgent.agentState;

    }

}
