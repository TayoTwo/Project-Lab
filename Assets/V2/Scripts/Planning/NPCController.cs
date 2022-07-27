using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    public PathFinder pathFinder;


    // Start is called before the first frame update
    void Start()
    {

        pathFinder = FindObjectOfType<PathFinder>();

    }

    // Update is called once per frame
    void Update()
    {
        
        //     Debug.Log(CalculateCost(Vector3.zero,targetPos.position));

    }
}
