namespace CSGoap
{
    using Godot;
    using System.Collections.Generic;

    public class FindCoverAction : GoapAction
    {
        public override string GetClazz()
        {
            return "FindCoverAction";
        }

        public override int GetCost(Dictionary<object, object> blackboard)
        {
            return 1;
        }

        public override Dictionary<object, object> GetPreconditions()
        {
            return new Dictionary<object, object>();
        }

        public override Dictionary<object, object> GetEffects()
        {
            return new Dictionary<object, object>
            {
                { "protected", true }
            };
        }

        public override bool Perform(Node2D actor, double delta)
        {
            Node2D closestCover = WorldState.Instance.GetClosestElement("cover", actor);

            if (closestCover == null)
            {
                return false;
            }

            if (closestCover.Position.DistanceTo(actor.Position) < 1)
            {
                return true;
            }

            // Assuming the actor has a method called MoveTo
            ((dynamic)actor).MoveTo(actor.Position.DirectionTo(closestCover.Position), delta);
            return false;
        }
    }
}