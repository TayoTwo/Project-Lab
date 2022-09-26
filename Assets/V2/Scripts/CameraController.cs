using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float smoothTime;
    Vector3 currentVel;

    public void ChangeTarget(Transform t){

        target = t;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null){

            transform.position = Vector3.SmoothDamp(transform.position,target.position + offset,ref currentVel,smoothTime);

        }

    }
}
