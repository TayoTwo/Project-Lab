using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public WorldButton button;
    public Vector3 origin;
    public Vector3 target;
    public float smoothTime;
    Vector3 targetPosition;
    Vector3 vel;

    bool canMove;
    public bool open;

    void Start(){

        origin = transform.position;
        target += transform.position;

    }

    void Update(){

        if(canMove){

            transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref vel,smoothTime);

        }

    }

    public void Open(){

        //Debug.Log("OPEN");
        targetPosition = target;
        canMove = true;

    }

    public void Close(){

       // Debug.Log("CLOSE");
        targetPosition = origin;
        canMove = true;
  
    }

}
