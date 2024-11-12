namespace CSGoap
{
   
    using Godot;
    using System.Collections.Generic;

    public class CalmDownAction : GoapAction
    {
        public override string GetClazz()
        {
            return "CalmDownAction";
        }

        public override int GetCost(Dictionary<object, object> blackboard)
        {
            return 1;
        }

        public override Dictionary<object, object> GetPreconditions()
        {
            return new Dictionary<object, object>
            {
                { "protected", true }
            };
        }

        public override Dictionary<object, object> GetEffects()
        {
            return new Dictionary<object, object>
            {
                { "is_frightened", false }
            };
        }

        public override bool Perform(Node2D actor, double delta)
        {
            // Assuming the actor has a method called CalmDown
            return ((dynamic)actor).CalmDown();
        }
    }
}