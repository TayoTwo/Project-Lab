using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectionTool : MonoBehaviour
{

    public GameObject selecteObject;
    public Vector3 mousePoint;
    InputMaster inputMaster;


    // Start is called before the first frame update
    void Awake()
    {

        inputMaster = new InputMaster();

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





    }
    //Ask agent to MoveTo/interact with object

    void OnRightClick(){



    }

    void OnDrawGizmos(){

        Gizmos.DrawWireSphere(mousePoint,1);

    }


}
