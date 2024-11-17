namespace CSGoap
{
   
    using Godot;
    using System.Collections.Generic;

    public partial class RelaxGoal : GoapGoal
    {
        public override string GetClazz()
        {
            return "RelaxGoal";
        }

        // Relax will always be available
        public override bool IsValid(GoapAgent agent)
        {
            return true;
        }

        // Relax has lower priority compared to other goals
        public override int Priority(GoapAgent agent)
        {
            return 0;
        }

        public override Dictionary<object, object> GetDesiredState()
        {
            return new Dictionary<object, object>();
        }
    }
}