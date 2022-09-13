using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public TMP_Text agentName;
    public TMP_Dropdown currentState;
    public TMP_Text currentGoal;
    public TMP_Text currentAction;
    public TMP_Text planText;
    public TMP_Dropdown actionDropdown;
    public SelectionTool selectionTool;
    public Button woodButton;
    public Button oreButton;
    public List<Toggle> toggleList = new List<Toggle>();

    void Start(){

        //woodButton.onClick.AddListener(delegate{selectionTool.OnBlockSelect(0);});
        //oreButton.onClick.AddListener(delegate{selectionTool.OnBlockSelect(1);});

    }

    // Update is called once per frame
    void LateUpdate()
    {

        UpdateUI();

    }

    public void OnResetButton(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    
    void UpdateUI(){

        if(selectionTool.selectedObject == null || selectionTool.selectedObject.GetComponent<Agent>() == null) return;

        Agent selectedAgent = selectionTool.selectedObject.GetComponent<Agent>();

        agentName.text = selectedAgent.gameObject.name;

        currentGoal.text = (selectedAgent.currentGoal == null)   ? "Current Goal: None" : "Current Goal: " + selectedAgent.currentGoal.goalName;
        currentAction.text = (selectedAgent.currentAction == null)   ? "Current Action: None" : "Current Action: " + selectedAgent.currentAction.actionName;

        toggleList[0].isOn = selectedAgent.worldState.Find(x => x.key == "hasWood").value;
        toggleList[1].isOn = selectedAgent.worldState.Find(x => x.key == "hasOre").value;
        toggleList[2].isOn = selectedAgent.worldState.Find(x => x.key == "hasShrooms").value;
        toggleList[3].isOn = selectedAgent.worldState.Find(x => x.key == "hasTool").value;
        toggleList[4].isOn = selectedAgent.worldState.Find(x => x.key == "hasHelped").value;
        toggleList[5].isOn = selectedAgent.worldState.Find(x => x.key == "hasPlacedBlock").value;
        toggleList[6].isOn = selectedAgent.worldState.Find(x => x.key == "isHungry").value;
        
        //Availible Actions

        actionDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();

        foreach(Action a in selectedAgent.actions){

            optionData.Add(new TMP_Dropdown.OptionData(a.actionName));

        }

        actionDropdown.AddOptions(optionData);

        //Display plan
        if(selectedAgent.plan != null){

            string planString = "";

            foreach(Action a in selectedAgent.plan.actions){

                planString += a.actionName + " > ";

            }

            planText.text = planString;

        } else {

            planText.text = null;

        }

        //Current 
        currentState.value = (int)selectedAgent.agentState;

    }

}
