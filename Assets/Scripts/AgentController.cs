using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    
    [Header("Movement")]
    public bool goingToA;
    public float changeTargetDis;
    public bool xAxis;
    public float distance;
    public Vector3 targetPos;
    public float patrolMoveSpeed;
    public float chaseMoveSpeed;
    Vector3 pointA;
    Vector3 pointB;

    [Header("Vision")]
    public float viewRadius;
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;


    [Header("Components")]
    public Light visionCone;
    NavMeshAgent navMeshAgent;
    //AnimationController animationController;

    [Header("Agent Attack System")]
    public float attackLag;
    public float attackRadius;
    public float damagePerTick;
    public GameObject dmgObject;
    public Health health;
    public bool isAttacking;
    public MeshRenderer dmgVisual;
    public MeshRenderer display;
    public AudioSource audioSource;
    public Transform head;
    bool hasDealtDmg;
    GOAPAgent agent;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the agents components
        navMeshAgent = GetComponent<NavMeshAgent>();
        //animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        agent = GetComponent<GOAPAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;
        audioSource = GetComponent<AudioSource>();

        //Set the patrol points
        if(xAxis){

            pointA = transform.position;
            pointB = (2 * distance * transform.right) + transform.position;

        } else {

            pointA = transform.position;
            pointB = (2 * distance * transform.forward) + transform.position;

        }
        
        //Go to point A
        ChangeTarget(pointA);

    }

    // Update is called once per frame
    void Update()
    {

        UpdateWorldState();

        visionCone.spotAngle = viewAngle;
        visionCone.range = viewRadius;

        if(isAttacking){

            if(!audioSource.isPlaying){

                audioSource.PlayOneShot(audioSource.clip);

            }

        } else {

            audioSource.Stop();

        }

    }

    void UpdateWorldState(){

        //If the player gameobject doesn't exist then set isPlayerDead to false otherwise set it to true
        if(player == null){

            agent.worldState.Find(x => x.key == "isPlayerDead").SetValue(true);

        } else {

            agent.worldState.Find(x => x.key == "isPlayerDead").SetValue(false);

        }

        //Check if the agent is healthy
        //agent.worldState.Find(x => x.key == "isHealthy").SetValue(health.isHealthy);

        CheckVisibleTargets();

    }

    void CheckVisibleTargets(){

        //Check for objects in the view radius
        Collider[] targetsInViewSphere = Physics.OverlapSphere(display.transform.position,viewRadius,targetMask);

        //Loop through our targets and check if we have direct line of sight of them
        for(int i = 0; i < targetsInViewSphere.Length;i++){

            Transform t = targetsInViewSphere[i].transform.root;
            Vector3 dir = (t.position - display.transform.position).normalized;

            if(Vector3.Angle(display.transform.forward,dir) < viewAngle / 2 && t.tag == "Player"){

                float dist = Vector3.Distance(t.position,display.transform.position);

                if(!Physics.Raycast(display.transform.position,dir,dist,obstacleMask)){

                    targetPos = t.position;
                    agent.worldState.Find(x => x.key == "canSeePlayer").SetValue(true);
                    return;

                }

            }

        }

        //Otherwise set canSeePlayer to false
        agent.worldState.Find(x => x.key == "canSeePlayer").SetValue(false);

    }

    public void Chase(){

        navMeshAgent.speed = chaseMoveSpeed;
        ChangeVisualsColor(Color.red);
        ChangeTarget(targetPos);

    }

    public void Patrol(){

        navMeshAgent.speed = patrolMoveSpeed;
        ChangeVisualsColor(Color.white);

        //Change the target if we get close
        if(goingToA){

            if(Vector3.Distance(pointA,transform.position) > changeTargetDis){

                ChangeTarget(pointA);

            } else {

                goingToA = false;

            }

        } else {

            if(Vector3.Distance(pointB,transform.position) > changeTargetDis){

                ChangeTarget(pointB);

            } else {

                goingToA = true;

            }

        }

    }

    public void FindHealthStation(){

        navMeshAgent.speed = chaseMoveSpeed;
        //Find every health station in the scene
        GameObject[] stations = GameObject.FindGameObjectsWithTag("HealthStation");
        Vector3 closestStation = stations[0].transform.position;

        //Find the closest one
        foreach(GameObject s in stations){

            float sDistance = Vector3.Distance(transform.position,s.transform.position);
            float cDistance = Vector3.Distance(transform.position,closestStation);

            if(sDistance < cDistance){

                closestStation = s.transform.position;

            }

        }

        ChangeVisualsColor(Color.green);
        ChangeTarget(closestStation);

    }

    public void ChangeTarget(Vector3 position){

        head.LookAt(position);
        targetPos = position;
        navMeshAgent.SetDestination(targetPos);

    }
    
    void OnTriggerStay(Collider other) {

        //If the player is in the damage sphere then update the inRangeOfPlayer state
        if(other.gameObject.tag == "Player"){

            dmgObject = other.transform.root.gameObject;
            agent.worldState.Find(x => x.key == "inRangeOfPlayer").SetValue(true);

            if(isAttacking && !hasDealtDmg){

                StartCoroutine(AttackCoroutine());
                
            }


        }
        
    }

    void OnTriggerExit(Collider other) {

        //If the player leaves the damage sphere then update the inRangeOfPlayer state
        if(other.gameObject.tag == "Player"){
            
            dmgObject = other.transform.root.gameObject;
            agent.worldState.Find(x => x.key == "inRangeOfPlayer").SetValue(false);
            isAttacking = false;

        }
        
    }

    public void Attack(){

        isAttacking = true;

    }

    IEnumerator AttackCoroutine(){

        dmgVisual.enabled = true;
        hasDealtDmg = true;

        Health hObj = dmgObject.GetComponent<Health>();

        hObj.TakeDamage(damagePerTick);

        yield return new WaitForSeconds(attackLag);

        dmgVisual.enabled = false;
        hasDealtDmg = false;

    }

    public void ChangeVisualsColor(Color color){

        // visionCone.color = color;

        // eyes[0].material.color = color;
        // eyes[1].material.color = color;

        // eyes[0].material.SetColor("_EmissionColor",color);
        // eyes[1].material.SetColor("_EmissionColor",color);

    }

    void OnDrawGizmos(){

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pointA,pointB);

        //View Radius
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,viewRadius);

        //
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,targetPos);

    }
}
