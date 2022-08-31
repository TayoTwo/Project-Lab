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
    public bool buildMode;
    public int blockType = -1;
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

                buildMode = false;
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

                if(buildMode){


                    if(agent.GetComponent<PlaceBlockAction>() != null){

                        PlaceBlockAction placeBlockAction = agent.GetComponent<PlaceBlockAction>();
                        placeBlockAction.preCons.Clear();

                        placeBlockAction.target = moveToPoint.transform;
                        placeBlockAction.blockType = blockType;
                        placeBlockAction.preCons.Add(new State("hasTool",true));

                        switch(blockType){

                            case 0:

                                placeBlockAction.preCons.Add(new State("hasWood",true));

                                break;
                            case 1:

                                placeBlockAction.preCons.Add(new State("hasOre",true));
                                break;

                            default:

                                Debug.Log("Block not selected");
                                break;

                        }

                    }

                    agent.ChangeGoal("PlaceBlock");

                } else {

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


    }

    public void OnBlockSelect(int b){

        blockType = b;
        buildMode = true;

    }

    void OnDrawGizmos(){

        if(buildMode){

            Color color = Color.red;

            switch(blockType){

                case 0:

                    color = Color.green;
                    break;
                case 1:
                    
                    color = Color.gray;
                    break;
                default:

                    color = Color.red;
                    break;

            }

            Vector3 roundedMousePoint = new Vector3(Mathf.RoundToInt(mousePoint.x),Mathf.RoundToInt(mousePoint.y),Mathf.RoundToInt(mousePoint.z));
            Gizmos.color = color;
            Gizmos.DrawWireCube(roundedMousePoint,Vector3.one);

        } else {

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(mousePoint,0.5f);

        }

    }


}
