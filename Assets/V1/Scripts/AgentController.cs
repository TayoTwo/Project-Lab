// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class AgentController : MonoBehaviour
// {
    
//     [Header("Movement")]
//     public int patrolIndex;
//     public float distance;
//     public Vector3 targetPos;
//     public float patrolMoveSpeed;
//     public float chaseMoveSpeed;
//     public Transform[] patrolTransforms;
//     public int maxPatrolPoints;
//     public List<Vector3> patrolPositions;
//     Vector3 lastSeenPlayerLocation;

//     [Header("Vision")]
//     public float viewRadius;
//     public float viewAngle;
//     public LayerMask targetMask;
//     public LayerMask agentMask;
//     public LayerMask obstacleMask;
//     public float changeTargetDis;
//     public float nearSupportRange;
//     public bool hasBeenCalled;

//     [Header("Components")]
//     public Light visionCone;
//     NavMeshAgent navMeshAgent;
//     //AnimationController animationController;

//     [Header("Agent Attack System")]
//     public float attackLag;
//     public float attackRadius;
//     public float damagePerTick;
//     public GameObject dmgObject;
//     public Health health;
//     public bool isAttacking;
//     public MeshRenderer dmgVisual;
//     public MeshRenderer display;
//     public AudioSource audioSource;
//     public Transform head;
//     bool hasDealtDmg;
//     //Call System
//     Vector3 callPosition;
//     GOAPAgent agent;
//     GameObject player;

//     // Start is called before the first frame update
//     void Awake()
//     {

//         //Initialize the agents components
//         navMeshAgent = GetComponent<NavMeshAgent>();
//         //animationController = GetComponent<AnimationController>();
//         health = GetComponent<Health>();
//         agent = GetComponent<GOAPAgent>();
//         player = GameObject.FindGameObjectWithTag("Player").gameObject;
//         audioSource = GetComponent<AudioSource>();

//         //Set the patrol points
//         foreach(Transform t in patrolTransforms){

//            AddPatrolPoint(t.position);

//         }
        
//         //Go to point A
//         ChangeTarget(patrolPositions[0]);

//     }

//     public int AgentCount(){

//         return GameObject.FindGameObjectsWithTag("Agent").Length;

//     }

//     // Update is called once per frame
//     void Update()
//     {

//         UpdateWorldState();

//         visionCone.spotAngle = viewAngle;
//         visionCone.range = viewRadius;

//         if(isAttacking){

//             if(!audioSource.isPlaying){

//                 audioSource.PlayOneShot(audioSource.clip);

//             }

//         } else {

//             audioSource.Stop();

//         }

//     }

//     void UpdateWorldState(){

//         //If the player gameobject doesn't exist then set isPlayerDead to false otherwise set it to true
//         if(player == null){

//             agent.worldState.Find(x => x.key == "isPlayerDead").SetValue(true);

//         } else {

//             agent.worldState.Find(x => x.key == "isPlayerDead").SetValue(false);

//         }

//         //Check if the agent is healthy
//         //agent.worldState.Find(x => x.key == "isHealthy").SetValue(health.isHealthy);

//         CheckVisibleTargets();

//     }

//     void CheckVisibleTargets(){

//         //Check for objects in the view radius
//         Collider[] targetsInViewSphere = Physics.OverlapSphere(display.transform.position,viewRadius,targetMask);

//         //Loop through our targets and check if we have direct line of sight of them
//         for(int i = 0; i < targetsInViewSphere.Length;i++){

//             Transform t = targetsInViewSphere[i].transform.root;
//             Vector3 dir = (t.position - display.transform.position).normalized;

//             if(Vector3.Angle(display.transform.forward,dir) < viewAngle / 2 && t.tag == "Player"){

//                 float dist = Vector3.Distance(t.position,display.transform.position);

//                 if(!Physics.Raycast(display.transform.position,dir,dist,obstacleMask)){

//                     targetPos = t.position;
//                     agent.worldState.Find(x => x.key == "canSeePlayer").SetValue(true);
//                     return;

//                 }

//             }

//         }

//         //Otherwise set canSeePlayer to false
//         agent.worldState.Find(x => x.key == "canSeePlayer").SetValue(false);

//     }

//     public void Chase(){

//         patrolIndex = patrolPositions.Count - 1;
//         lastSeenPlayerLocation = targetPos;
//         navMeshAgent.speed = chaseMoveSpeed;
//         ChangeVisualsColor(Color.red);
//         ChangeTarget(targetPos);

//     }

//     void AddPatrolPoint(Vector3 position){

//         if(patrolPositions.Count >= maxPatrolPoints){

//             patrolPositions.RemoveAt(0);

//         }

//         patrolPositions.Add(position);

//     }

//     public void Patrol(){

//         if(!patrolPositions.Exists(x => x == lastSeenPlayerLocation) && lastSeenPlayerLocation != Vector3.zero){

//             patrolPositions[patrolPositions.Count - 1] = lastSeenPlayerLocation;

//         }

//         navMeshAgent.speed = patrolMoveSpeed;
//         ChangeVisualsColor(Color.white);

//         //Change the target if we get close
//         if(Vector3.Distance(patrolPositions[patrolIndex],transform.position) > changeTargetDis){

//             ChangeTarget(patrolPositions[patrolIndex]);

//         } else {

//             if(patrolIndex + 1 < maxPatrolPoints){

//                 patrolIndex++;

//             } else {

//                 patrolIndex = 0;

//             }

//         }

//     }

//     public void SetCaller(AgentController c){

//         hasBeenCalled = true;
//         callPosition = c.transform.position;

//     }

//     public void Respond(){

//         if(Vector3.Distance(patrolPositions[patrolIndex],transform.position) > nearSupportRange){

//             agent.worldState.Find(x => x.key == "closeToCallPos").SetValue(false);
//             ChangeTarget(callPosition);

//         } else {

//             agent.worldState.Find(x => x.key == "closeToCallPos").SetValue(true);

//         }


//     }

//     public void Call(){

//         ChangeTarget(transform.position);

//         Collider[] targetsInViewSphere = Physics.OverlapSphere(display.transform.position,viewRadius,agentMask);
//         navMeshAgent.speed = chaseMoveSpeed;

//         //Loop through our targets and check if we have direct line of sight of them
//         for(int i = 0; i < targetsInViewSphere.Length;i++){

//             if( targetsInViewSphere[i].transform.root == transform) continue;

//             Transform t = targetsInViewSphere[i].transform.root;
//             Vector3 dir = (t.position - display.transform.position).normalized;

//             if(t.tag == "Agent"){

//                 float dist = Vector3.Distance(t.position,display.transform.position);

//                 if(!Physics.Raycast(display.transform.position,dir,dist,obstacleMask)){

//                     t.GetComponent<AgentController>().SetCaller(this);
//                     Debug.Log("CALLING: " + t.name);

//                 }

//                 if(dist < nearSupportRange){

//                     agent.worldState.Find(x => x.key == "getSupport").SetValue(true); 
//                     t.GetComponent<AgentController>().hasBeenCalled = false;  

//                 } else {

//                     agent.worldState.Find(x => x.key == "getSupport").SetValue(false);   

//                 }

//             }

//         }


//     }

//     public void FindHealthStation(){

//         navMeshAgent.speed = chaseMoveSpeed;
//         //Find every health station in the scene
//         GameObject[] stations = GameObject.FindGameObjectsWithTag("HealthStation");
//         Vector3 closestStation = stations[0].transform.position;

//         //Find the closest one
//         foreach(GameObject s in stations){

//             float sDistance = Vector3.Distance(transform.position,s.transform.position);
//             float cDistance = Vector3.Distance(transform.position,closestStation);

//             if(sDistance < cDistance){

//                 closestStation = s.transform.position;

//             }

//         }

//         ChangeVisualsColor(Color.green);
//         ChangeTarget(closestStation);

//     }

//     public void ChangeTarget(Vector3 position){

//         head.LookAt(position);
//         targetPos = position;
//         navMeshAgent.SetDestination(targetPos);

//     }
    
//     void OnTriggerStay(Collider other) {

//         //If the player is in the damage sphere then update the inRangeOfPlayer state
//         if(other.gameObject.tag == "Player"){

//             dmgObject = other.transform.root.gameObject;
//             agent.worldState.Find(x => x.key == "inRangeOfPlayer").SetValue(true);

//             if(isAttacking && !hasDealtDmg){

//                 StartCoroutine(AttackCoroutine());
                
//             }


//         }
        
//     }

//     void OnTriggerExit(Collider other) {

//         //If the player leaves the damage sphere then update the inRangeOfPlayer state
//         if(other.gameObject.tag == "Player"){
            
//             dmgObject = other.transform.root.gameObject;
//             agent.worldState.Find(x => x.key == "inRangeOfPlayer").SetValue(false);
//             isAttacking = false;

//         }
        
//     }

//     public void Attack(){

//         isAttacking = true;

//     }

//     IEnumerator AttackCoroutine(){

//         dmgVisual.enabled = true;
//         hasDealtDmg = true;

//         Health hObj = dmgObject.GetComponent<Health>();

//         hObj.TakeDamage(damagePerTick);

//         yield return new WaitForSeconds(attackLag);

//         dmgVisual.enabled = false;
//         hasDealtDmg = false;

//     }

//     public void ChangeVisualsColor(Color color){

//         // visionCone.color = color;

//         // eyes[0].material.color = color;
//         // eyes[1].material.color = color;

//         // eyes[0].material.SetColor("_EmissionColor",color);
//         // eyes[1].material.SetColor("_EmissionColor",color);

//     }

//     // void OnDrawGizmos(){

//     //     for(int i = 0;i < maxPatrolPoints - 1;i++){

//     //         Gizmos.color = Color.blue;
//     //         Gizmos.DrawLine(patrolPositions[i],patrolPositions[i+1]);

//     //     }



//     //     //View Radius
//     //     Gizmos.color = Color.white;
//     //     Gizmos.DrawWireSphere(transform.position,viewRadius);

//     //     //
//     //     Gizmos.color = Color.red;
//     //     Gizmos.DrawLine(transform.position,targetPos);

//     // }
// }
