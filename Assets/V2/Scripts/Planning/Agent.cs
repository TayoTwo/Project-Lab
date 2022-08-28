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
    public bool isIdle;
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
    public GridManager gridManager;
    public Backpack backpack;
    public Hunger hunger;
    ActionPlanner actionPlanner;
    NavMeshAgent navMeshAgent;
    AnimationController animationController;
    Goal highestPriorityGoal;

    void Awake(){

        gridManager = FindObjectOfType<GridManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        actionPlanner = GetComponent<ActionPlanner>();
        animationController = GetComponent<AnimationController>();
        backpack = GetComponent<Backpack>();

        //Set actions by looking through components
        //Set goals
        //Add all relevant effects to world state
        Action[] foundActions = gameObject.GetComponents<Action>();
        //Debug.Log(foundActions.Length);
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

    //GetBestGoal

    public Goal GetBestGoal(){

        List<Goal> validGoals = new List<Goal>();
        validGoals.Clear();

        //Make a list of valid goals
        foreach(Goal g in goals){

            if(g.isValid(worldState)){

                validGoals.Add(g);

            }

        }

        //Debug.Log("Valid GOALS " + validGoals.Count);

        if(validGoals.Count == 0){

            Debug.Log("NO VALID GOALS");
            currentGoal = null;
            return null;

        }

        highestPriorityGoal = null;

        //Loop through our list of valid goals and check which has the highest priority
        foreach(Goal g in validGoals){

            if(highestPriorityGoal != null && g.priority > highestPriorityGoal.priority){

                highestPriorityGoal = g;

            }

        }

        return highestPriorityGoal;

    }

    //Essentially the plan maker
    void IdleState(){

        //Debug.Log(gameObject.name + " IDLING");

        navMeshAgent.isStopped = true;

        if(currentGoal == null || !currentGoal.isValid(worldState)){

            currentGoal = GetBestGoal();

        }

        isIdle = (currentGoal == null) ? true : false;

        if(currentGoal != null || currentAction == null || !currentAction.isValid()){

            plan = actionPlanner.FindBestPlan(currentGoal);

        }

        if(plan != null && plan.actions.Count != 0){

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

        } else if(!isIdle){

            agentState = AgentState.SIGNAL;
            return;

        }



    }

    //Move to the actions location
    void MoveToState(){

        //Debug.Log("MOVING");

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(currentAction.target.position);

        if(currentAction.needsRangeCheck() && currentAction.target == null){

            agentState = AgentState.IDLE;
            Debug.Log("TARGET IS NULL");
            return;

        }

        if(isWithinRange(currentAction.target.position)){

            agentState = AgentState.PERFORM;
            //Debug.Log("IN RANGE TO PERFORM");
            return;

        }

        //NavMesh move too

    }

    //Perform the action
    void PerformActionState(){

        //Debug.Log("PERFORMING");

        navMeshAgent.isStopped = true;

        if(plan == null){

            agentState = AgentState.IDLE;
            return;

        }

        // if(currentAction.needsRangeCheck() && currentAction.target != null && !isWithinRange(currentAction.target.position)){

        //     agentState = AgentState.MOVETO;
        //     return;

        // }

        //Until the action has been complete continue to do it
        bool stepComplete = currentAction.perform(this);

        //If the step has been completed then go to the next step in the plan
        if(stepComplete && currentPlanStep < plan.actions.Count - 1){

            currentPlanStep++;
            currentAction = plan.actions[currentPlanStep];
            agentState = AgentState.MOVETO;

        } else if(stepComplete && currentPlanStep >= plan.actions.Count - 1){

            Debug.Log("PLAN COMPLETE");
            currentGoal = null;
            currentAction = null;
            plan = null;
            currentPlanStep = 0;
            agentState = AgentState.IDLE;

        }

        if(!currentAction.isValid()){

            agentState = AgentState.IDLE;
            return;

        }

    }
    void SignalState(){

        //Debug.Log("SIGNALING");

        List<State> statesToComplete = new List<State>(currentGoal.desiredWorldState);
        
        //Loop through the possible actions and check if their effects would get us to our desired world state
        foreach(Action action in actions){

            List<State> effects = action.getEffects();
            //If the action's effects match our desired world state then remove it to check what else needs to be satisfied
            //We also mark the current action as needed in the plan
            foreach(State state in statesToComplete.ToArray()){

                if(!effects.Exists(x => x.key == state.key)) continue;

                if(effects.Exists(x => x.key == state.key) && effects.Find(x => x.key == state.key).value.Equals(state.value)){

                    //Debug.Log("HELLO");
                    statesToComplete.Remove(state);
                    List<State> preCons = action.getPrecons();

                    foreach(State s in preCons){

                        //Debug.Log(state.key + "-" + state.value);
                        statesToComplete.Add(s);

                    }

                }
 
            }   

        }

        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        List<(Agent,bool)> helpers = new List<(Agent,bool)>();

        foreach(GameObject a in agents){

            if(a == gameObject) continue;
            if(a.GetComponent<Agent>().currentGoal != null) continue;

            Agent agentComp = a.GetComponent<Agent>();
            bool canHelp = false;

            (statesToComplete,canHelp) = agentComp.RespondToSignal(statesToComplete,this);

            helpers.Add((agentComp,canHelp));

        }

        foreach(State state in statesToComplete.ToArray()){

            if(worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                statesToComplete.Remove(state);

            } else {

                //Debug.Log("NEED TO COMPELTE" + state.key);

            }

        }

        //Debug.Log("STATES TO COMPLETE " + gameObject.name + " " + statesToComplete.Count);
        
        if(statesToComplete.Count == 0){

            agentState = AgentState.IDLE;

            // foreach((Agent,bool) h in helpers){

            //     h.Item1.RespondToSignal(statesToComplete,this);

            // }

            return;

        }

        //REQUEST FOR OTHER AGENTS TO FILL REQUEST
        //ASKS AGENTS THAT CAN RESPOND TO REQUEST TO CHANGE THEIR GOAL TO 'RespondToRequest'
        //THIS IS COMPLETED BY COMPLETING AS MANY OF THE ASKERS WORLD STATE

    }

    public (List<State> , bool) RespondToSignal(List<State> statesToComplete, Agent caller){

        //Debug.Log(caller.name + " " + caller.agentState);

        if(caller.agentState != AgentState.SIGNAL) {

            Debug.Log("CALLER HAS BEEN SATISFIED");

            caller = null;
            ResetAgent();

        }

        bool canHelp = false;
        List<State> statesLeft = new List<State>(statesToComplete);
        List<State> toDoList = new List<State>();

        //Loop through the possible actions and check if their effects would get us to our desired world state
        foreach(Action action in actions){

            List<State> effects = action.getEffects();
            //Debug.Log(action.actionName + ":" + effects.Count);

            //If the action's effects match our desired world state then remove it to check what else needs to be satisfied
            //We also mark the current action as needed in the plan
            foreach(State state in statesToComplete.ToArray()){

                //Debug.Log(state.key + "-" + state.value);
                if(!effects.Exists(x => x.key == state.key)) continue;

                if(effects.Find(x => x.key == state.key).value.Equals(state.value)){

                    //Debug.Log(gameObject.name + " CAN HELP " + caller.name);
                    toDoList.Add(state);
                    statesLeft.Remove(state);
                    canHelp = true;

                }
 
            } 

        }

        if(canHelp){

            //Change the responders goal and create a new plan
            currentAction = GetComponent<GiveAction>();
            currentAction.preCons = toDoList;
            currentAction.target = caller.transform;

            currentGoal = goals.Find(x => x.goalName == "Help");

            agentState = AgentState.IDLE;

        } else {

            caller = null;
            ResetAgent();

        }

        return (statesLeft, canHelp);
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

    public void ChangeGoal(string goalName){

        if(!goals.Find(x => x.goalName == goalName).isValid(worldState)) return;

        currentGoal = goals.Find(x => x.goalName == goalName);

        agentState = AgentState.IDLE;

    }

    void ResetAgent(){

        currentGoal = null;
        currentAction = null;
        plan = null;
        currentPlanStep = 0;
        agentState = AgentState.IDLE;

    }

    void OnDrawGizmos(){

        if(navMeshAgent == null) return;

        Debug.DrawLine(transform.position,navMeshAgent.destination);

    }
    
}
