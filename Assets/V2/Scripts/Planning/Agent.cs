using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState {

    IDLE,
    MOVETO,
    PERFORM

}

public class Agent : MonoBehaviour
{

    //FSM
    
    [Header("Live Agent Information")]
    public AgentState agentState;
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
    ActionPlanner actionPlanner;
    NavMeshAgent navMeshAgent;
    Goal highestPriorityGoal;

    void Awake(){

        //Set actions by looking through components
        //Set goals
        //Add all relevant effects to world state
        Action[] foundActions = gameObject.GetComponents<Action>();
        foreach(Action action in foundActions){

            actions.Add(action);

            List<State> effects = action.getEffects();

            foreach(State state in effects){

                if(effects.Exists(x => x.key == state.key)) continue;

                if(!effects.Find(x => x.key == state.key).value.Equals(state.value)){

                    worldState.Add(state);

                } 

            }

        }

    }

    void Update(){

        switch(agentState){

            case AgentState.IDLE:

                IdleState();
                break;

            case AgentState.MOVETO:

                MoveToState();
                break;
            
            case AgentState.PERFORM:

                PerformActionState();
                break;

        }

    }

    //Essentially the plan maker
    void IdleState(){

        navMeshAgent.SetDestination(transform.position);
        plan = actionPlanner.FindBestPlan(currentGoal);
        currentPlanStep = 0;
        currentAction = plan.actions[currentPlanStep];
        currentActionName = plan.actions[currentPlanStep].actionName;

        if(plan != null){

            agentState = AgentState.PERFORM;

        }

    }

    //Move to the actions location
    void MoveToState(){

        if(currentAction.needsRangeCheck() && currentAction.target == null){

            agentState = AgentState.IDLE;
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

    public List<Action> getActions(){

        return actions;

    }
}
