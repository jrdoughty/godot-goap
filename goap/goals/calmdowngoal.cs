namespace CSGoap
{
    using Godot;
    using System.Collections.Generic;

    public class CalmDownGoal : GoapGoal
    {
        public override string GetClazz()
        {
            return "CalmDownGoal";
        }

        public override bool IsValid()
        {
            return (bool)WorldState.Instance.GetState("is_frightened", false);
        }

        public override int Priority()
        {
            return 10;
        }

        public override Dictionary<object, object> GetDesiredState()
        {
            return new Dictionary<object, object>
            {
                { "is_frightened", false }
            };
        }
    }
}