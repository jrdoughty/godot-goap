namespace CSGoap
{
	
	using Godot;
	using System.Collections.Generic;

	public partial class GoapAgent : Node
	{
		private List<GoapGoal> goals;
		private GoapGoal currentGoal;
		private List<GoapAction> currentPlan;
		private int currentPlanStep = 0;
		private Node2D actor;

		public List<GoapAction> actions;
		public Dictionary<object, object> state = new Dictionary<object, object>();


		public void SetActions(List<GoapAction> actions)
		{
			this.actions = actions;
			GD.Print("Actions set");
		}

		public object GetState(object key, object defaultValue = null)
		{
			if (state.ContainsKey(key))
			{
				return state[key];
			}
			else if (defaultValue != null)
			{
				state[key] = defaultValue;
			}
						
			return defaultValue;
		}

		public void SetState(object key, object value)
		{
			state[key] = value;
		}


		public override void _Ready()
		{
			actor = GetParent<Node2D>();
		}
		public void SetGoals(List<GoapGoal> goals)
		{
			this.goals = goals;
		}

		public override void _Process(double delta)
		{
			GoapGoal goal = GetBestGoal();
			if (currentGoal == null || goal != currentGoal)
			{
				currentGoal = goal;
				currentPlan = Goap.Instance.GetActionPlanner().GetPlan(currentGoal, this);
				currentPlanStep = 0;
			}
			else
			{
				FollowPlan(currentPlan, delta);
			}
		}

		public GoapGoal GetBestGoal()
		{
			GoapGoal highestPriority = null;

			foreach (GoapGoal goal in goals)
			{
				if (goal.IsValid(this) && (highestPriority == null || goal.Priority(this) > highestPriority.Priority(this)))
				{
					highestPriority = goal;
				}
			}
			return highestPriority;
		}

		private void FollowPlan(List<GoapAction> plan, double delta)
		{
			if (plan == null || plan.Count == 0)
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
}
