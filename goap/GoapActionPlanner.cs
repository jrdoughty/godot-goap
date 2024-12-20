namespace CSGoap
{
   
	using Godot;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using System.Linq;

    public partial class GoapActionPlanner : Node
	{


		public Dictionary<object, object> GetCombinedState(GoapAgent agent)
		{
			Dictionary<object, object> combinedState = new Dictionary<object, object>(WorldState.Instance.state);
			foreach (KeyValuePair<object, object> s in agent.state)
			{
				if (combinedState.ContainsKey(s.Key))
				{
					combinedState[s.Key] = s.Value;
				}
				else
				{
					combinedState.Add(s.Key, s.Value);
				}
			}
			return combinedState;
		}

		public List<GoapAction> GetPlan(GoapGoal goal, GoapAgent agent)
		{

			WorldState.Instance.ConsoleMessage($"Goal: {goal.GetClazz()}");

			Dictionary<object, object> desiredState = new Dictionary<object, object>(goal.GetDesiredState());

			if (desiredState.Count == 0)
			{
				return new List<GoapAction>();
			}
			var plan = FindBestPlan(goal, desiredState, agent);
			PrintPlan(plan);
			return plan;
		}

		private List<GoapAction> FindBestPlan(GoapGoal goal, Dictionary<object, object> desiredState, GoapAgent agent)
		{
			var root = new Dictionary<object, object>
			{
				{ "action", goal },
				{ "state", desiredState },
				{ "children", new List<Dictionary<object, object>>() }
			};
			Dictionary<object, object> blackboard = GetCombinedState(agent);

			if (BuildPlans(root, blackboard, agent))
			{
				return ExtractPlan(root);
			}

			return new List<GoapAction>();
		}

		private bool BuildPlans(Dictionary<object, object> root, Dictionary<object, object> blackboard, GoapAgent agent)
		{
			bool hasFollowUp = false;

			Dictionary<object, object> state = new Dictionary<object, object>(root["state"] as Dictionary<object, object>);
			
			foreach( KeyValuePair<Object,Object> s in root["state"] as Dictionary<object, object>)
			{
				if(state.ContainsKey(s.Key) && blackboard.ContainsKey(s.Key) && state[s.Key].Equals(blackboard[s.Key]))
				{
					state.Remove(s.Key);
				}
			}
			if (state.Count == 0)
			{
				return true;
			}
			foreach (GoapAction a in agent.actions)
			{
				if (!a.IsValid(agent))
				{
					continue;
				}

				bool shouldUseAction = false;
				Dictionary<object,object> effects = a.GetEffects();
				var desiredState = new Dictionary<object, object>(state);
				foreach (KeyValuePair<object,object> s in desiredState)
				{
					if (effects.ContainsKey(s.Key) && effects[s.Key].Equals(s.Value))
					{
						shouldUseAction = true;
						desiredState.Remove(s.Key);
					}
				}

				if (shouldUseAction)
				{
					var preconditions = a.GetPreconditions();
					foreach (KeyValuePair<object, object> p in preconditions)
					{
						desiredState[p.Key] = p.Value;
					}
					Dictionary<object, object> s = new Dictionary<object, object>{
						{ "action", a }, 
						{ "state", desiredState }, 
						{ "children", new List<Dictionary<object, object>>() }
					};
					/*
					foreach (KeyValuePair<object, object> d in desiredState)
					{
						GD.Print($"Desired state: {d.Key} {d.Value}");
					}*/
					if(desiredState.Count == 0 || BuildPlans(s, new Dictionary<object, object>(blackboard), agent))
					{
						(root["children"] as List<Dictionary<object, object>>).Add(s);
						hasFollowUp = true;
					}
				}
			}
			return true;
		}

		private List<GoapAction> ExtractPlan(Dictionary<object, object> root)
		{
			var plan = new List<GoapAction>();
			var current = root;

			while (current != null && current.ContainsKey("children") && (current["children"] as List<Dictionary<object, object>>).Count > 0)
			{
				var children = current["children"] as List<Dictionary<object, object>>;
				var nextStep = children[0]; // Assuming the first child is the next step in the plan
				var action = nextStep["action"] as GoapAction;

				if (action != null)
				{
					plan.Insert(0,action);
				}

				current = nextStep;
			}
			return plan.ToList();
		}

		public void PrintPlan(List<GoapAction> plan)
		{
			String planString = "Plan: ";
			foreach (GoapAction a in plan)
			{
				planString += a.GetClazz() + " "+ a.GetCost(null) + " ";
			}
				WorldState.Instance.ConsoleMessage(planString);
		}
	}
}
