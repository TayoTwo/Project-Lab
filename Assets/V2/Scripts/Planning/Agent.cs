using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState {

    IDLE,
    MOVETO,
    PERFORM,
    SIGNAL

}

public class Agent : MonoBehaviour
{

    //FSM
    public AgentState agentState;
    public float closeToTargetDis;
    
    [Header("Live Agent Information")]
    public Plan plan;
    public int currentPlanStep;
    public Goal currentGoal;
    public Action currentAction;
    public string currentActionName;
    public List<State> worldState;
    [Header("Static Data")]
    //public AgentSettings agentSettings;
    public List<Goal> goals;
    public List<Action> actions = new List<Action>();
    public PathFinder pathFinder;
    public Backpack backpack;
    ActionPlanner actionPlanner;
    NavMeshAgent navMeshAgent;
    AnimationController animationController;
    Goal highestPriorityGoal;

    void Awake(){

        pathFinder = FindObjectOfType<PathFinder>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        actionPlanner = GetComponent<ActionPlanner>();
        animationController = GetComponent<AnimationController>();
        backpack = GetComponent<Backpack>();

        //Set actions by looking through components
        //Set goals
        //Add all relevant effects to world state
        Action[] foundActions = gameObject.GetComponents<Action>();
        Debug.Log(foundActions.Length);
        foreach(Action action in foundActions){

            actions.Add(action);

        //     List<State> effects = action.getEffects();

        //      foreach(State state in effects){

        //          if(effects.Exists(x => x.key == state.key)) continue;

        //          if(!effects.Find(x => x.key == state.key).value.Equals(state.value)){

        //              worldState.Add(state);

        //          } 

        //    }

        }

    }

    void Update(){

        switch(agentState){

            case AgentState.IDLE:

                animationController.moving = false;
                animationController.interacting = false;
                animationController.signaling = false;
                IdleState();
                break;

            case AgentState.MOVETO:

                animationController.moving = true;
                animationController.interacting = false;
                animationController.signaling = false;
                MoveToState();
                break;
            
            case AgentState.PERFORM:

                animationController.moving = false;
                animationController.interacting = true;
                animationController.signaling = false;
                PerformActionState();
                break;
            case AgentState.SIGNAL:

                animationController.moving = false;
                animationController.interacting = false;
                animationController.signaling = true;
                SignalState();
                break;

        }

    }

    //Essentially the plan maker
    void IdleState(){

        navMeshAgent.SetDestination(transform.position);
        plan = actionPlanner.FindBestPlan(currentGoal);

        if(plan != null){

            currentPlanStep = 0;
            currentAction = plan.actions[currentPlanStep];
            currentActionName = plan.actions[currentPlanStep].actionName;

            if(!currentAction.needsRangeCheck()){

                agentState = AgentState.PERFORM;
                return;

            } else {

                agentState = AgentState.MOVETO;
                return;

            }

        } else {

            agentState = AgentState.SIGNAL;

        }



    }

    //Move to the actions location
    void MoveToState(){

        if(currentAction.needsRangeCheck() && currentAction.target == null){

            agentState = AgentState.IDLE;
            Debug.Log("TARGET IS NULL");
            return;

        }

        if(isWithinRange(currentAction.target.position)){

            agentState = AgentState.PERFORM;
            Debug.Log("IN RANGE TO PERFORM");
            return;

        }

        //NavMesh move too
        navMeshAgent.SetDestination(currentAction.target.position);

    }

    //Perform the action
    void PerformActionState(){

        if(plan == null){

            agentState = AgentState.IDLE;

        }

        //Until the action has been complete continue to do it
        bool stepComplete = currentAction.perform(this);

        //If the step has been completed then go to the next step in the plan
        if(stepComplete && currentPlanStep < plan.actions.Count - 1){

            currentPlanStep++;
            currentAction = plan.actions[currentPlanStep];

        } else if(currentPlanStep >= plan.actions.Count - 1){

            agentState = AgentState.IDLE;

        }

    }

    void SignalState(){

        Debug.Log("SIGNALING FOR HELP");
        List<State> statesToComplete = new List<State>(currentGoal.desiredWorldState);
        
        //Loop through the possible actions and check if their effects would get us to our desired world state
        foreach(Action action in actions){

            List<State> effects = action.getEffects();
            //If the action's effects match our desired world state then remove it to check what else needs to be satisfied
            //We also mark the current action as needed in the plan
            foreach(State state in statesToComplete.ToArray()){

                if(!effects.Exists(x => x.key == state.key)) continue;

                if(effects.Find(x => x.key == state.key).value.Equals(state.value)){

                    statesToComplete.Remove(state);

                }
 
            }   

        }

        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");

        foreach(GameObject a in agents){

            Agent agentComp = a.GetComponent<Agent>();

            statesToComplete = agentComp.RespondToSignal(statesToComplete,this);

            if(statesToComplete.Count == 0){

                return;

            }


        }

        //REQUEST FOR OTHER AGENTS TO FILL REQUEST
        //ASKS AGENTS THAT CAN RESPOND TO REQUEST TO CHANGE THEIR GOAL TO 'RespondToRequest'
        //THIS IS COMPLETED BY COMPLETING AS MANY OF THE ASKERS WORLD STATE

    }

    public List<State> RespondToSignal(List<State> statesToComplete, Agent caller){

        bool canHelp = false;
        List<State> statesLeft = new List<State>(statesToComplete);
        List<State> toDoList = new List<State>();

        //Loop through the possible actions and check if their effects would get us to our desired world state
        foreach(Action action in actions){

            List<State> effects = action.getEffects();

            //If the action's effects match our desired world state then remove it to check what else needs to be satisfied
            //We also mark the current action as needed in the plan
            foreach(State state in statesToComplete.ToArray()){

                if(!effects.Exists(x => x.key == state.key)) continue;

                if(effects.Find(x => x.key == state.key).value.Equals(state.value)){

                    toDoList.Add(state);
                    statesLeft.Remove(state);
                    canHelp = true;

                }
 
            } 

        }

        Debug.Log(canHelp);

        toDoList.Add(new State("hasHelped",true)); 

        if(canHelp){

            currentGoal = goals.Find(x => x.goalName == "Help");
            currentAction = GetComponent<GiveAction>();
            currentAction.preCons = toDoList;
            currentAction.target = caller.transform;

        }

        return statesLeft;
    }

    public bool isWithinRange(Vector3 targetPos){

        //Debug.Log(Vector3.Distance(transform.position,targetPos));

        if(Vector3.Distance(transform.position,targetPos) < closeToTargetDis){

            return true;

        }

        return false;

    }

    public List<Action> getActions(){

        return actions;

    }
}
