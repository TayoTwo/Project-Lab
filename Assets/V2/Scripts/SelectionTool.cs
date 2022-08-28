using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTool : MonoBehaviour
{

    public GameObject selectedObject;
    public Vector3 mousePoint;
    public UIManager uIManager;
    public GameObject actionPanel;
    public int goalIndex;
    public List<Goal> goals;
    InputMaster inputMaster;
    GameObject moveToPoint;

    // Start is called before the first frame update
    void Awake()
    {

        inputMaster = new InputMaster();
        inputMaster.Player.LeftClick.performed += ctx => OnLeftClick();
        inputMaster.Player.RightClick.performed += ctx => OnRightClick();
        actionPanel.SetActive(false);

    }

    
    void OnEnable(){

        inputMaster.Enable();

    }

    void OnDisable(){

        inputMaster.Disable();

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(inputMaster.Player.MousePosition.ReadValue<Vector2>().x,inputMaster.Player.MousePosition.ReadValue<Vector2>().y,Camera.main.nearClipPlane));
        RaycastHit hitPoint;

        if(Physics.Raycast(ray,out hitPoint)){

            mousePoint = hitPoint.point;
            Debug.DrawLine(Camera.main.transform.position,mousePoint);


        }

    }

    //Select diselect an agent
    void OnLeftClick(){

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(inputMaster.Player.MousePosition.ReadValue<Vector2>().x,inputMaster.Player.MousePosition.ReadValue<Vector2>().y,Camera.main.nearClipPlane));
        RaycastHit hitPoint;
        Transform hitObject;

        if(Physics.Raycast(ray,out hitPoint)){

            hitObject = hitPoint.transform.root;
            //Debug.Log("Hit " + hitObject.name);

            selectedObject = hitObject.gameObject;

            if(selectedObject.GetComponent<Agent>() != null){

                actionPanel.SetActive(true);

            } else {

                actionPanel.SetActive(false);

            }

        }

    }
    //Ask agent to MoveTo/interact with object

    void OnRightClick(){

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(inputMaster.Player.MousePosition.ReadValue<Vector2>().x,inputMaster.Player.MousePosition.ReadValue<Vector2>().y,Camera.main.nearClipPlane));
        RaycastHit hitPoint;
        moveToPoint = new GameObject();

        if(Physics.Raycast(ray,out hitPoint)){

            //Debug.Log("Right clicked " + hitPoint.transform.root.tag);

            moveToPoint.transform.position = hitPoint.point;

            if(selectedObject.GetComponent<Agent>() != null){

                Agent agent = selectedObject.GetComponent<Agent>();
                agent.currentAction = null;

                switch(hitPoint.transform.root.tag){

                    case "Tree":

                        agent.ChangeGoal("GetWood");

                        if(agent.GetComponent<CutTreeAction>() != null){

                            agent.GetComponent<CutTreeAction>().target = hitPoint.transform.root;

                        }
                        break;

                    case "Ore":

                        agent.ChangeGoal("GetOre");

                        if(agent.GetComponent<MineOreAction>() != null){

                            agent.GetComponent<MineOreAction>().target = hitPoint.transform.root;

                        }
                        break;

                    case "Shroom":

                        agent.ChangeGoal("GetShroom");

                        if(agent.GetComponent<PickShroomAction>() != null){

                            //Debug.Log("Selected Shroom");
                            agent.GetComponent<PickShroomAction>().target = hitPoint.transform.root;

                        }

                        break;

                    case "Workbench":

                        agent.ChangeGoal("MakeTool");

                        if(agent.GetComponent<MakeToolAction>() != null){

                            agent.GetComponent<MakeToolAction>().target = hitPoint.transform.root;

                        }
                        break;

                    default:

                        //Debug.Log("[" + hitPoint.transform.root.tag + "]");

                        agent.ChangeGoal("MoveTo");
                        agent.GetComponent<MoveToPointAction>().target = moveToPoint.transform;

                        break;

                }

            }

        }


    }

    void OnDrawGizmos(){

        Gizmos.DrawWireSphere(mousePoint,1);

    }


}
