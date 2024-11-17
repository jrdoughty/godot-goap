namespace CSGoap
{
   
    using Godot;
    using System.Collections.Generic;

    public partial class KeepFirepitBurningGoal : GoapGoal
    {
        public override string GetClazz()
        {
            return "KeepFirepitBurningGoal";
        }

        public override bool IsValid(GoapAgent agent)
        {
	        return WorldState.Instance.GetElements("firepit").Count == 0;
        }

        public override int Priority(GoapAgent agent)
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