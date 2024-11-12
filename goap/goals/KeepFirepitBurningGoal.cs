namespace CSGoap
{
   
    using Godot;
    using System.Collections.Generic;

    public class KeepFirepitBurningGoal : GoapGoal
    {
        public override string GetClazz()
        {
            return "KeepFirepitBurningGoal";
        }

        public override bool IsValid()
        {
            return (bool)WorldState.Instance.GetState("has_firepit", false);
        }

        public override int Priority()
        {
            return 5;
        }

        public override Dictionary<object, object> GetDesiredState()
        {
            return new Dictionary<object, object>
            {
                { "has_firepit", true },
                { "has_wood", true }
            };
        }
    }
}