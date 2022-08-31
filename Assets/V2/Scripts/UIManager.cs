using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public TMP_Text agentName;
    public TMP_Dropdown currentState;
    public TMP_Text currentGoal;
    public TMP_Text currentAction;
    public TMP_Dropdown actionDropdown;
    public SelectionTool selectionTool;
    public Button woodButton;
    public Button oreButton;

    void Start(){

        //woodButton.onClick.AddListener(delegate{selectionTool.OnBlockSelect(0);});
        //oreButton.onClick.AddListener(delegate{selectionTool.OnBlockSelect(1);});

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

        currentGoal.text = (selectedAgent.currentGoal == null)   ? "Current Goal: None" : "Current Goal: " + selectedAgent.currentGoal.goalName;
        currentAction.text = (selectedAgent.currentAction == null)   ? "Current Action: None" : "Current Action: " + selectedAgent.currentAction.actionName;
        
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
