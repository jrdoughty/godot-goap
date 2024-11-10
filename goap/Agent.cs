using Godot;
using System.Collections.Generic;

public class GoapAgent : Node
{
    private Goal[] goals;
    private Goal currentGoal;
    private List<Action> currentPlan;
    private int currentPlanStep = 0;
    private Node2D actor;

    public GoapAgent(Node2D actor, Goal[] goals)
    {
        this.actor = actor;
        this.goals = goals;
    }

    public override void _Process(float delta)
    {
        Goal goal = GetBestGoal();
        if (currentGoal == null || goal != currentGoal)
        {
            Dictionary<object, object> blackboard = new Dictionary<object, object>
            {
                { "position", actor.Position }
            };

            foreach (var state in WorldState.instance().GetState())
            {
                blackboard[state.Key] = state.Value;
            }

            currentGoal = goal;
            currentPlan = Goap.instace().GetActionPlanner().GetPlan(currentGoal, blackboard);
            currentPlanStep = 0;
        }
        else
        {
            FollowPlan(currentPlan, delta);
        }
    }

    public Goal GetBestGoal()
    {
        Goal highestPriority = null;

        foreach (Goal goal in goals)
        {
            if (goal.IsValid() && (highestPriority == null || goal.Priority() > highestPriority.Priority()))
            {
                highestPriority = goal;
            }
        }

        return highestPriority;
    }

    private void FollowPlan(List<Action> plan, float delta)
    {
        if (plan.Count == 0)
        {
            return;
        }

        bool isStepComplete = plan[currentPlanStep].Perform(actor, delta);
        if (isStepComplete && currentPlanStep < plan.Count - 1)
        {
            currentPlanStep++;
        }
    }
}