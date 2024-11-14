namespace CSGoap
{
    using Godot;
    using System;
    using System.Collections.Generic;

    public partial class FindFoodAction : GoapAction
    {
        public override string GetClazz()
        {
            return "FindFoodAction";
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
                { "is_hungry", false }
            };
        }

        public override bool Perform(Node2D actor, double delta)
        {
            Node2D closestFood = WorldState.Instance.GetClosestElement("food", actor);

            if (closestFood == null)
            {
                return false;
            }

            if (closestFood.Position.DistanceTo(actor.Position) < 5)
            {
                double hunger = (double)WorldState.Instance.GetState("hunger");
                WorldState.Instance.SetState("hunger", hunger - (closestFood as Mushroom).nutrition);
                closestFood.QueueFree();
                return true;
            }

            // Assuming the actor has a method called MoveTo
            ((dynamic)actor).MoveTo(actor.Position.DirectionTo(closestFood.Position), delta);
            return false;
        }
    }
}