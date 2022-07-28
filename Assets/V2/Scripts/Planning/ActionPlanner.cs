using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlanner : MonoBehaviour
{

    public Agent agent;
    public void Awake(){

        agent = GetComponent<Agent>();

    }

    public Plan FindBestPlan(Goal currentGoal){

        List<State> dws = currentGoal.desiredWorldState;
        
        if(dws == null){

            return null;

        }

        (TreeNode<PlanStep> tree, bool hasSolution) =  BuidPlanTree(new TreeNode<PlanStep>(new PlanStep(agent.currentAction,dws)));

        if(!hasSolution){

            //Debug.Log("Couldn't Find Valid Tree");
            return null;

        } else {
            
            //Debug.Log("Found Valid Tree");
            List<Plan> plans = treeToPlan(tree);

            //PrintPlan(plans[0]);

            return GetCheapestPlan(plans);

        }


    }

    List<Plan> treeToPlan(TreeNode<PlanStep> tree){

        List<Plan> plans = new List<Plan>();

        //If the tree doesn't have any children then just add the roots action to the plan
        if(tree.children.Count == 0){

            Action a = tree.value.action;

            plans.Add(new Plan(new List<Action>(){a}, tree.value.action.getCost(agent.worldState,agent)));

            return plans;

        }

        //Loop through every branch in the tree and add its actions to the plan
        foreach(TreeNode<PlanStep> branch in tree.children){

            foreach(Plan childPlan in treeToPlan(branch)){

                childPlan.AddToPlan(tree.value.action,tree.value.action.getCost(agent.worldState,agent));
                plans.Add(childPlan);


            }

        }
       
        return plans;

    }

    public Plan GetCheapestPlan(List<Plan> plans){

        Plan bestPlan = plans[0];

        //Loop through each plan and chechk which has the lowest total cost
        foreach(Plan plan in plans){

            //PrintPlan(plan);
            if(plan.cost < bestPlan.cost){

                bestPlan = plan;

            }

        }

        return bestPlan;

    }

    public (TreeNode<PlanStep>, bool) BuidPlanTree(TreeNode<PlanStep> stepTree){

        bool hasSolution = false;

        List<State> desiredWorldState = new List<State>(stepTree.value.desiredState);

        //If the desired world state has any states in common with the current world state then remove it
        foreach(State state in desiredWorldState.ToArray()){

            Debug.Log(state.key);
            if(agent.worldState.Find(x => x.key == state.key).value.Equals(state.value)){

                //Debug.Log("STATE ALREADY SATISFIED");
                desiredWorldState.Remove(state);

            }

        }
        //Loop through the possible actions and check if their effects would get us to our desired world state
        foreach(Action action in agent.getActions()){

            bool shouldUseAction = false;
            List<State> effects = action.getEffects();
            List<State> tempDesiredWorldState = new List<State>(desiredWorldState);

            //If the action's effects match our desired world state then remove it to check what else needs to be satisfied
            //We also mark the current action as needed in the plan
            foreach(State state in tempDesiredWorldState.ToArray()){

                if(!effects.Exists(x => x.key == state.key)) continue;

                if(effects.Find(x => x.key == state.key).value.Equals(state.value)){

                    tempDesiredWorldState.Remove(state);
                    shouldUseAction = true;

                }
 
            }   

            //If the action should be used in the plan then add its preconditions to the desired world state
            //We then check in turn what steps are needed to get these preconditions
            if(shouldUseAction){

                List<State> preConditions = action.getPrecons();

                foreach(State state in preConditions){
                    
                    tempDesiredWorldState.Add(state);

                }

                //Debug.Log("Action " + action.actionName + " is valid");

                (TreeNode<PlanStep> branch, bool branchHasSolution) = BuidPlanTree(
                    new TreeNode<PlanStep>(
                        new PlanStep(action,
                        new List<State>(tempDesiredWorldState))));

                if(tempDesiredWorldState.Count == 0 || branchHasSolution){

                    //Debug.Log("Adding to tree " + action.actionName);
                    stepTree.children.AddLast(branch);   
                    hasSolution = true;

                }

            }

        }

        return (stepTree,hasSolution);

    }

    void PrintPlan(Plan plan){

        //Debug.Log("PLAN - Steps:" + plan.actions.Count);        

        foreach(Action a in plan.actions){

            Debug.Log(a.actionName);

        }

    }

}
