namespace CSGoap
{
	
	using Godot;
	using System.Collections.Generic;

	public partial class GoapAgent : Node
	{
		private GoapGoal[] goals;
		private GoapGoal currentGoal;
		private List<GoapAction> currentPlan;
		private int currentPlanStep = 0;
		private Node2D actor;

		public GoapAgent(Node2D actor, GoapGoal[] goals)
		{
			this.actor = actor;
			this.goals = goals;
		}

		public override void _Process(double delta)
		{
			GoapGoal goal = GetBestGoal();
			if (currentGoal == null || goal != currentGoal)
			{
				Dictionary<object, object> blackboard = new Dictionary<object, object>
				{
					{ "position", actor.Position }
				};

				blackboard = new Dictionary<object, object>(WorldState.Instance.state);

				currentGoal = goal;
				currentPlan = Goap.Instance.GetActionPlanner().GetPlan(currentGoal, blackboard);
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
				if (goal.IsValid() && (highestPriority == null || goal.Priority() > highestPriority.Priority()))
				{
					highestPriority = goal;
				}
			}

			return highestPriority;
		}

		private void FollowPlan(List<GoapAction> plan, double delta)
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
}
