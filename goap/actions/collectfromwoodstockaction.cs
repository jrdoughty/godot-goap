namespace CSGoap
{
   
	using Godot;
	using System.Collections.Generic;

	public partial class CollectFromWoodStockAction : GoapAction
	{
		public override string GetClazz()
		{
			return "CollectFromWoodStockAction";
		}

		public override bool IsValid()
		{
			return WorldState.Instance.GetElements("wood_stock").Count > 0;
		}

		public override int GetCost(Dictionary<object, object> blackboard)
		{
			if (blackboard.ContainsKey("position"))
			{
				Node2D closestTree = WorldState.Instance.GetClosestElement("wood_stock", (Node2D)blackboard["position"]);
				return (int)(closestTree.Position.DistanceTo(((Node2D)blackboard["position"]).Position) / 5);
			}
			return 5;
		}

		public override Dictionary<object, object> GetPreconditions()
		{
			return new Dictionary<object, object>();
		}

		public override Dictionary<object, object> GetEffects()
		{
			return new Dictionary<object, object>
			{
				{ "has_wood", true }
			};
		}

		public override bool Perform(Node2D actor, double delta)
		{
			Node2D closestStock = WorldState.Instance.GetClosestElement("wood_stock", actor);

			if (closestStock != null)
			{
				if (closestStock.Position.DistanceTo(actor.Position) < 10)
				{
					closestStock.QueueFree();
					WorldState.Instance.SetState("has_wood", true);
					return true;
				}
				else
				{
					// Assuming the actor has a method called MoveTo
					((dynamic)actor).MoveTo(actor.Position.DirectionTo(closestStock.Position), delta);
				}
			}

			return false;
		}
	}
}
