// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GOAPAgent : MonoBehaviour
// {

//     [Header("Live Agent Information")]
//     public Plan plan;
//     public int currentPlanStep;
//     public GOAPGoal currentGoal;
//     public GOAPAction currentAction;
//     public List<GOAPState> worldState;
//     [Header("Static Data")]
//     public AgentSettings agentSettings;
//     List<GOAPGoal> goals;
//     List<GOAPAction> actions = new List<GOAPAction>();
//     GOAPActionPlanner actionPlanner;
//     GOAPGoal highestPriorityGoal;

//     void Awake(){

//         //plan = new Plan();
//         InitializeWorldState();
//         actionPlanner = GetComponent<GOAPActionPlanner>();
//         currentAction = actions[0];

//     }

    
//     void InitializeWorldState(){

//         actions = agentSettings.actions;
//         goals = agentSettings.goals;

//         foreach(GOAPAction action in actions){

//             List<GOAPState> effects = action.getEffects();

//             foreach(GOAPState state in effects){

//                 if(effects.Exists(x => x.key == state.key)) continue;

//                 if(!effects.Find(x => x.key == state.key).value.Equals(state.value)){

//                     worldState.Add(state);

//                 } 

//             }

//         }



//     }

//     void Update(){

//         //Update the best goal
//         GOAPGoal bestGoal = GetBestGoal();

//         if(plan == null){

//             Debug.Log("PLAN IS NULL");

//         } else {

//             Debug.Log("PLAN IS NOT NULL");

//         }

//         //If the plan is empty then find a new plan
//         //If the current goal is not the best goal or the current goal/action is invalid then change the plan
//         if(plan is null){

//             if(plan.actions.Count == 0
//             || currentGoal != bestGoal 
//             || !currentGoal.isValid(this) 
//             || !currentAction.isValid(this)){

//                 currentGoal = bestGoal;
//                 plan = actionPlanner.FindBestPlan(currentGoal);
//                 currentPlanStep = 0;
//                 currentAction = plan.actions[currentPlanStep];

//                 return;

//             }
            
//         }

//         FollowPlan();

//     }

//     public GOAPGoal GetBestGoal(){

//         List<GOAPGoal> validGoals = new List<GOAPGoal>();
//         validGoals.Clear();

//         //Make a list of valid goals
//         foreach(GOAPGoal g in goals){

//             if(g.isValid(this)){

//                 validGoals.Add(g);

//             }

//         }

//         if(validGoals.Count == 0){

//             Debug.Log("NO VALID GOALS");
//             return null;

//         }

//         highestPriorityGoal = validGoals[0];

//         //Loop through our list of valid goals and check which has the highest priority
//         foreach(GOAPGoal g in validGoals){

//             if(g.priority > highestPriorityGoal.priority){

//                 highestPriorityGoal = g;

//             }

//         }

//         return highestPriorityGoal;

//     }

//     public void FollowPlan(){

//         if(plan is null){

//             return;

//         }

//         //Until the action has been complete continue to do it
//         bool stepComplete = currentAction.perform(this);

//         //If the step has been completed then go to the next step in the plan
//         if(stepComplete && currentPlanStep < plan.actions.Count - 1){

//             currentPlanStep++;
//             currentAction = plan.actions[currentPlanStep];

//         }

//     }

//     public List<GOAPGoal> getGoals(){

//         return goals;

//     }
//     public List<GOAPAction> getActions(){

//         return actions;

//     }

// }
