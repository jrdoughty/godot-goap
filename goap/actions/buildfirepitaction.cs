namespace CSGoap
{
   
    using Godot;
    using System.Collections.Generic;

    public class BuildFirepitAction : GoapAction
    {
        private PackedScene Firepit = (PackedScene)ResourceLoader.Load("res://scenes/firepit.tscn");

        public override string GetClazz()
        {
            return "BuildFirepitAction";
        }

        public override int GetCost(Dictionary<object, object> blackboard)
        {
            return 1;
        }

        public override Dictionary<object, object> GetPreconditions()
        {
            return new Dictionary<object, object>
            {
                { "has_wood", true }
            };
        }

        public override Dictionary<object, object> GetEffects()
        {
            return new Dictionary<object, object>
            {
                { "has_firepit", true }
            };
        }

        public override bool Perform(Node2D actor, double delta)
        {
            Node2D closestSpot = WorldState.Instance.GetClosestElement("firepit_spot", actor);

            if (closestSpot == null)
            {
                return false;
            }

            if (closestSpot.Position.DistanceTo(actor.Position) < 20)
            {
                Node2D firepit = (Node2D)Firepit.Instance();
                actor.GetParent().AddChild(firepit);
                firepit.Position = closestSpot.Position;
                firepit.ZIndex = closestSpot.ZIndex;
                WorldState.Instance.SetState("has_wood", false);
                return true;
            }

            actor.MoveTo(actor.Position.DirectionTo(closestSpot.Position), delta);

            return false;
        }
    }
}