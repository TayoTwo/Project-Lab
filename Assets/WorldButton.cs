using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    public bool toggleButton;
    public MovingPlatform platform;
    public BoxCollider boxCollider;
    public LayerMask layerMask;
    public bool triggered;

    // Start is called before the first frame update
    void Start()
    {

        layerMask = 1 << layerMask;
        
    }

    // Update is called once per frame
    void Update()
    {

        Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center,boxCollider.size/2,Quaternion.identity,layerMask);

        if(colliders.Length > 0 && !triggered){

            Debug.Log(colliders[0].name);

            MoveDown();

        } else {

            MoveUp();

        }
        
    }

    public void MoveDown(){

        platform.Open();

    }

    public void MoveUp(){

        platform.Close();

    }

    void OnDrawGizmos(){

        Gizmos.DrawCube(transform.position + boxCollider.center,boxCollider.size);

    }

}
