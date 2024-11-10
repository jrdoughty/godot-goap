//
// Planner. Goap's heart.
//


class GoapActionPlanner extends Node
{
	public Action[] _actions;


	//
	// set actions available for planning.
	// this can be changed in runtime for more dynamic options.
	//
	void set_actions(Action[] actions)
	{
		_actions = actions;
	}


	//
	// Receives a Goal and an optional blackboard.
	// Returns a list of actions to be executed.
	//
	// This functiontion is the core of the planner. It will build a graph
	// with actions and their pre-conditions and effects. It will then
	// try to find the best path to achieve the desired state.
	//
	// Returns an array of actions.
	//
	// This functiontion is recursive. It will call itself until it finds
	// a solution or until it can't find any more actions to add to the graph.
	//
	// This functiontion is not protected from circular dependencies. This is
	// easy to implement though.
	//
		public Action[] GetPlan(Goal goal, Dictionary<object,object> blackboard = new Dictionary<object,object>())
		{
			GD.Print("Goal: %s" % goal.get_clazz());
			WorldState.ConsoleMessage("Goal: %s" % goal.get_clazz());
			Dictionary<object, object> desired_state = goal.get_desired_state().duplicate();

			if (desired_state.Count == 0)
			{
				return [];
			}
			return FindBestPlan(goal, desired_state, blackboard)
		}


	function FindBestPlan(Goal goal, Dictionary<object,object>  desired_state, Dictionary<object,object>  blackboard)
	{
		// goal is set as root action. It does feel weird
		// but the code is simpler this way.
		Dictionary<object,object>  root = {
			"action": goal,
			"state": desired_state,
			"children": []
		}

		// build plans will populate root with children.
		// In case it doesn't find a valid path, it will return false.
		if (BuildPlans(root, blackboard.duplicate()))
		{
			var plans = TransformTreeIntoArray(root, blackboard);
			return GetCheapestPlan(plans);
		}
		return []

	}
	//
	// Compares plan's cost and returns
	// actions included in the cheapest one.
	//
	Action[] GetCheapestPlan(plans)
	{
		Action[] bestPlan;
		foreach (p in plans)
		{
			PrintPlan(p);
			if (bestPlan == null or p.cost < bestPlan.cost)
			{
				bestPlan = p;
			}
		}
		return bestPlan.actions;
	}

	//
	// Builds graph with actions. Only includes valid plans (plans
	// that achieve the goal).
	//
	// Returns true if the path has a solution.
	//
	// This functiontion uses recursion to build the graph. This is
	// necessary because any new action included in the graph may
	// add pre-conditions to the desired state that can be satisfied
	// by previously considered actions, meaning, on every step we
	// need to iterate from the beginning to find all solutions.
	//
	// Be aware that for simplicity, the current implementation is not protected from
	// circular dependencies. This is easy to implement though.
	//
	public bool BuildPlans(step, blackboard)
	{
		bool has_followup = false;

		// each node in the graph has it's own desired state.
		var state = step.state.duplicate()
		// checks if the blackboard contains data that can
		// satisfy the current state.
		foreach (s in step.state)
		{
			if (state[s] == blackboard.get(s))
			{
				state.erase(s);
			}
		}
		// if the state is empty, it means this branch already
		// found the solution, so it doesn't need to look for
		// more actions
		if (state.is_empty())
		{
			return true;
		}
		foreach (action in _actions)
		{
			if (!action.IsValid())
			{
				continue;
			}
			bool should_use_action = false;
			Dictionary<object,object> effects = action.get_effects();
			Dictionary<object,object> desired_state = state.duplicate();

			// check if action should be used, i.e. it
			// satisfies at least one condition from the
			// desired state
			foreach (s in desired_state)
			{
				if (desired_state[s] == effects.get(s))
				{
					desired_state.erase(s);
					should_use_action = true;
				}
			}
			if (should_use_action)
			{
				// adds actions pre-conditions to the desired state
				Dictionary<object,object> preconditions = action.get_preconditions()
				foreach (p in preconditions)
				{
					desired_state[p] = preconditions[p];
				}
				Dictionary<object,object> s = {
					"action": action,
					"state": desired_state,
					"children": []
					}

				// if desired state is empty, it means this action
				// can be included in the graph.
				// if it's not empty, BuildPlans is called again (recursively) so
				// it can try to find actions to satisfy this current state. In case
				// it can't find anything, this action won't be included in the graph.
				if desired_state.is_empty() or BuildPlans(s, blackboard.duplicate()):
					step.children.push_back(s);
					has_followup = true;
			}
		}
		return has_followup;
	}

	//
	// Transforms graph with actions into list of actions and calculates
	// the cost by summing actions' cost
	//
	// Returns list of plans.
	//
	Action[] TransformTreeIntoArray(p, blackboard)
	{
		var plans = []

		if p.children.size() == 0:
			plans.push_back({ "actions": [p.action], "cost": p.action.get_cost(blackboard) })
			return plans

		foreach (c in p.children)
		{
			foreach (child_plan in TransformTreeIntoArray(c, blackboard))
			{
				if (p.action.has_method("get_cost"))
				{
					child_plan.actions.push_back(p.action);
					child_plan.cost += p.action.get_cost(blackboard);
				}
				plans.push_back(child_plan);
			}
		}
		return plans
	}

	//
	// Prints plan. Used for Debugging only.
	//
	void PrintPlan(plan)
	{
		var actions = []
		foreach (a in plan.actions)
		{
			actions.push_back(a.get_clazz());
		}
		print({"cost": plan.cost, "actions": actions});
		WorldState.ConsoleMessage({"cost": plan.cost, "actions": actions});
	}
}