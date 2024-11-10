//
// This script integrates the actor (NPC) with goap.
// In your implementation you could have this logic
// inside your NPC script.
//
// As good practice, I suggest leaving it isolated like
// this, so it makes re-use easy and it doesn't get tied
// to unrelated implementation details (movement, collisions, etc)


class GoapAgent extends Node
{
    Goal[] goals;
    Goal currentGoal;
    Action[] currentPlan;
    int currentPlanStep = 0;

    object actor;

    //
    // On every loop this script checks if the current goal is still
    // the highest priority. if it's not, it requests the action planner a new plan
    // for the new high priority goal.
    //
    override public void _Process(float delta)
    {
        Goal goal = GetBestGoal()
        if(currentGoal == null or goal != currentGoal)
        {
        // You can set in the blackboard any relevant information you want to use
        // when calculating action costs and status. I'm not sure here is the best
        // place to leave it, but I kept here to keep things simple.
            Dictionary<object,object> blackboard = {
                "position": actor.position,
                }

            foreach (s in WorldState._state)
            {
                blackboard[s] = WorldState._state[s];
            }
            currentGoal = goal;
            currentPlan = Goap.GetActionPlanner().GetPlan(currentGoal, blackboard);
            currentPlanStep = 0;
        }
        else
        {
            FollowPlan(currentPlan, delta)
        }

    }
    public GoapAgent(object actor, Goal[] goals)
    {
        this.actor = actor;
        this.goals = goals;
    }


    //
    // Returns the highest priority goal available.
    //

    public Goal GetBestGoal()
    {
        var highestPriority = null;

        foreach (var goal in goals)
        {
            if (goal.IsValid() && (highestPriority == null || goal.Priority() > highestPriority.Priority()))
            {
                highestPriority = goal;
            }
        }

        return highestPriority;
    }


    //
    // Executes plan. This functiontion is called on every game loop.
    // "plan" is the current list of actions, and delta is the time since last loop.
    //
    // Every action exposes a functiontion called perform, which will return true when
    // the job is complete, so the agent can jump to the next action in the list.
    //

    public void FollowPlan(GoapAction[] plan, float delta)
    {
        if (plan.Length == 0)
        {
            return;
        }

        if (currentPlanStep >= plan.Length)
        {
            return;
        }

        bool isStepComplete = plan[currentPlanStep].Perform(actor, delta);

        if (isStepComplete && _currentPlanStep < plan.Length - 1)
        {
            _currentPlanStep++;
        }
    }
}