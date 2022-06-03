using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{

    [Header("Camera Settings")]
    public float sens;
    public bool invertedY;
    public bool invertedX;
    public bool lockCursor;
    //Local variables
    Vector2 cursorDelta;
    Camera playerCamera;
    InputMaster inputMaster;
    float cursorX;
    float cursorY;

    void OnEnable(){

        inputMaster.Enable();

    }

    void OnDisable(){

        inputMaster.Disable();

    }

    // Start is called before the first frame update
    void Awake(){
        
        playerCamera = GetComponentInChildren<Camera>();
        inputMaster = new InputMaster();

    }

    // Update is called once per frame
    void Update()
    {
        if(lockCursor) {
            
            Cursor.lockState = CursorLockMode.Locked;
            
        } else { 
             
            Cursor.lockState = CursorLockMode.None;

        }


        cursorDelta = inputMaster.Player.CursorDelta.ReadValue<Vector2>();
        cursorX += cursorDelta.x * sens * -2 *(Convert.ToInt32(invertedX) - 0.5f);
        cursorY += cursorDelta.y * sens * 2 *(Convert.ToInt32(invertedY) - 0.5f);
 
        cursorY = Mathf.Clamp(cursorY, -90f, 90f);      
 
        //Camera rotation only allowed if game us not paused
        playerCamera.transform.localRotation = Quaternion.Euler(cursorY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, cursorX, 0f);

        //Debug.Log(playerCamera.transform.eulerAngles.x);
        
    }
}
