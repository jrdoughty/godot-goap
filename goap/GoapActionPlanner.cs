namespace CSGoap
{
   
	using Godot;
	using System.Collections.Generic;
    using System.Linq;

    public partial class GoapActionPlanner : Node
	{
		private GoapAction[] actions;

		public void SetActions(GoapAction[] actions)
		{
			this.actions = actions;
		}

		public List<GoapAction> GetPlan(GoapGoal goal, Dictionary<object, object> blackboard = null)
		{
			GD.Print("planning!");
			if (blackboard == null)
			{
				blackboard = new Dictionary<object, object>();
			}

			GD.Print($"Goal: {goal.GetClazz()}");
			WorldState.Instance.ConsoleMessage($"Goal: {goal.GetClazz()}");

			Dictionary<object, object> desiredState = new Dictionary<object, object>(goal.GetDesiredState());

			if (desiredState.Count == 0)
			{
				return new List<GoapAction>();
			}

			return FindBestPlan(goal, desiredState, blackboard);
		}

		private List<GoapAction> FindBestPlan(GoapGoal goal, Dictionary<object, object> desiredState, Dictionary<object, object> blackboard)
		{
			var root = new Dictionary<string, object>
			{
				{ "action", goal },
				{ "state", desiredState },
				{ "children", new List<Dictionary<string, object>>() }
			};

			if (BuildPlans(root, new Dictionary<object, object>(blackboard)))
			{
				return ExtractPlan(root);
			}

			return new List<GoapAction>();
		}

		private bool BuildPlans(Dictionary<string, object> root, Dictionary<object, object> blackboard)
		{
			var steps = new Stack<Dictionary<string, object>>();
			steps.Push(root);

			while (steps.Count > 0)
			{
				var step = steps.Pop();
				var action = step["action"] as GoapAction;
				var state = step["state"] as Dictionary<object, object>;
				var children = step["children"] as List<Dictionary<string, object>>;

				foreach (var a in actions)
				{
					if (!a.IsValid())
					{
						continue;
					}

					var preconditions = a.GetPreconditions();
					var effects = a.GetEffects();

					if (Satisfies(state, preconditions))
					{
						var newState = new Dictionary<object, object>(state);
						foreach (var key in effects.Keys)
						{
							newState[key] = effects[key];
						}

						var newStep = new Dictionary<string, object>
						{
							{ "action", a },
							{ "state", newState },
							{ "children", new List<Dictionary<string, object>>() }
						};
						children.Add(newStep);
						steps.Push(newStep);
					}
				}
			}

			return true;
		}

		private bool Satisfies(Dictionary<object, object> state, Dictionary<object, object> preconditions)
		{
			foreach (var precondition in preconditions)
			{
				if (!state.ContainsKey(precondition.Key) || !state[precondition.Key].Equals(precondition.Value))
				{
					return false;
				}
			}
			return true;
		}

		private List<GoapAction> ExtractPlan(Dictionary<string, object> root)
		{
			var plan = new List<GoapAction>();
			var current = root;

			while (current != null && current.ContainsKey("children") && (current["children"] as List<Dictionary<string, object>>).Count > 0)
			{
				var children = current["children"] as List<Dictionary<string, object>>;
				var nextStep = children[0]; // Assuming the first child is the next step in the plan
				var action = nextStep["action"] as GoapAction;

				if (action != null)
				{
					plan.Add(action);
				}

				current = nextStep;
			}
			return plan.ToList();
		}
	}
}
