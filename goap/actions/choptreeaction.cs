namespace CSGoap
{

    using Godot;
    using System.Collections.Generic;

    public class ChopTreeAction : GoapAction
    {
        public override string GetClazz()
        {
            return "ChopTreeAction";
        }

        public override bool IsValid()
        {
            return WorldState.Instance.GetElements("tree").Length > 0;
        }

        public override int GetCost(Dictionary<object, object> blackboard)
        {
            if (blackboard.ContainsKey("position"))
            {
                Node2D closestTree = WorldState.Instance.GetClosestElement("tree", (Node2D)blackboard["position"]);
                return (int)(closestTree.Position.DistanceTo(((Node2D)blackboard["position"]).Position) / 7);
            }
            return 3;
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
            Node2D closestTree = WorldState.Instance.GetClosestElement("tree", actor);

            if (closestTree != null)
            {
                if (closestTree.Position.DistanceTo(actor.Position) < 10)
                {
                    // Assuming the actor has a method called ChopTree
                    if (((dynamic)actor).ChopTree(closestTree))
                    {
                        WorldState.Instance.SetState("has_wood", true);
                        return true;
                    }
                    return false;
                }
                else
                {
                    // Assuming the actor has a method called MoveTo
                    ((dynamic)actor).MoveTo(actor.Position.DirectionTo(closestTree.Position), delta);
                }
            }

            return false;
        }
    }
}