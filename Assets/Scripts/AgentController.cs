using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform head;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        ChangeTarget(FindObjectOfType<PlayerController>().transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        ChangeTarget(FindObjectOfType<PlayerController>().transform.position);

    }

    public void ChangeTarget(Vector3 position){

        head.LookAt(position);
        navMeshAgent.SetDestination(position);

    }
}
