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
    Agent caller;
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
        hunger = GetComponent<Hunger>();

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

            if(g.isValid(this)){

                validGoals.Add(g);

            }

        }

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


        navMeshAgent.isStopped = true;

        //Find the best goal
        if(currentGoal == null || !currentGoal.isValid(this)){

            currentGoal = GetBestGoal();

        }

        //If we still can't find a goal then idle
        isIdle = (currentGoal == null) ? true : false;

        //Find a plan for the current goal or replan if the current action is invalid
        if(currentGoal != null || currentAction == null || !currentAction.isValid(this)){

            plan = actionPlanner.FindBestPlan(currentGoal);

        }

        //Run the plan
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

        //If we can't fullfull our goal then signal for help
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

        if(currentAction.needsRangeCheck() && (currentAction.target == null || !currentAction.isValid(this))){

            agentState = AgentState.IDLE;
            Debug.Log("TARGET IS NULL");
            return;

        }

        if(isWithinRange(currentAction.target.position) || !currentAction.needsRangeCheck()){

            agentState = AgentState.PERFORM;
            //Debug.Log("IN RANGE TO PERFORM");
            return;

        }

    }

    //Perform the action
    void PerformActionState(){

        //Debug.Log("PERFORMING");

        navMeshAgent.isStopped = true;

        if(plan == null){

            agentState = AgentState.IDLE;
            return;

        }

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
            return;

        }
        
        if(!currentAction.isValid(this)){

            agentState = AgentState.IDLE;
            return;

        }

        if(currentAction.needsInRange  && !isWithinRange(currentAction.target.position)){

            agentState = AgentState.MOVETO;

        }

    }
    
    void SignalState(){

        //Debug.Log("SIGNALING");

        List<State> statesToComplete = new List<State>(currentGoal.desiredWorldState);
        
        //Loop through our states to complete and find what preconditions need to be added
        for(int i = 0;i < statesToComplete.Count;i++){

            State s = statesToComplete[i];

            if(worldState.Find(x => x.key == s.key).value.Equals(s.value)){

                continue;

            }

            //If an action can help our states to complete add its preconditions
            foreach(Action action in actions){

                List<State> preCons = action.getPrecons();
                List<State> effects = action.getEffects();

                if(effects.Exists(x => x.key == s.key) && effects.Find(x => x.key == s.key).value.Equals(s.value)){
                    //Debug.Log("ACTION IS VALID AND HAS PRECON COUNT " + preCons.Count);
                    statesToComplete.Remove(s);

                    foreach(State p in preCons){

                        if(!statesToComplete.Exists(x => x.key == p.key)){

                            //Debug.Log("ADDED PRECON " + p.key);
                            statesToComplete.Add(p);


                        }

                    }

                    //Debug.Log("RESTARTING LOOP");
                    i = -1;

                }

            }


        }

        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        List<(Agent,bool)> helpers = new List<(Agent,bool)>();
        //Find all agents that can help
        foreach(GameObject a in agents){

            if(a == gameObject) continue;
            if(a.GetComponent<Agent>().currentGoal != null) continue;

            Agent agentComp = a.GetComponent<Agent>();
            bool canHelp = false;

            (statesToComplete,canHelp) = agentComp.RespondToSignal(statesToComplete,this);

            helpers.Add((agentComp,canHelp));

        }
        //Remove an item from our to-do list if our worldstate is satisfied
        foreach(State state in statesToComplete.ToArray()){

            if(worldState.Find(x => x.key == state.key).value == state.value){

                statesToComplete.Remove(state);

            } 

        }
        //If our to-do list is empty then we've completed our goal
        if(statesToComplete.Count == 0){

            Debug.Log("SIGNAL HAS BEEN SATISFIED");
            agentState = AgentState.IDLE;

            statesToComplete.Clear();
            return;

        }



    }

    public (List<State> , bool) RespondToSignal(List<State> statesToComplete, Agent c){

        caller = c;

        //If the current goal has a higher priority ignore the signal
        if(currentGoal != null && currentGoal.priority > goals.Find(x => x.goalName =="Help").priority) return (statesToComplete,false);

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

            //If the action's effects match our desired world state then remove it to check what else needs to be satisfied
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
            currentGoal = goals.Find(x => x.goalName == "Help");

            currentAction = GetComponent<GiveAction>();
            currentAction.preCons = toDoList;
            currentAction.target = caller.transform;

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

        if(!goals.Find(x => x.goalName == goalName).isValid(this)) return;

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
