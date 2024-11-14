namespace CSGoap
{
    using System;
    using Godot;
    using System.Collections.Generic;

    public partial class KeepFedGoal : GoapGoal
    {
        public override string GetClazz()
        {
            return "KeepFedGoal";
        }

        // This is not a valid goal when hunger is less than 50.
        public override bool IsValid()
        {
            return Convert.ToInt32(WorldState.Instance.GetState("hunger", 0)) > 50 && WorldState.Instance.GetElements("food").Count > 0;
        }

        public override int Priority()
        {
            return Convert.ToInt32(WorldState.Instance.GetState("hunger", 0)) < 75 ? 1 : 2;
        }

        public override Dictionary<object, object> GetDesiredState()
        {
            return new Dictionary<object, object>
            {
                { "is_hungry", false }
            };
        }
    }
}