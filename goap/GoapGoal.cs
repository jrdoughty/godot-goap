namespace CSGoap
{
    //
    // Goal contract
    //
    using Godot;
    using System.Collections.Generic;
    public class GoapGoal : Node
    {


        public virtual string GetClazz()
        {
            return "GoapGoal";
        }

        //
        // This indicates if the goal should be considered or not.
        // Sometimes instead of changing the priority, it is easier to
        // not even consider the goal. i.e. Ignore combat related goals
        // when there are not enemies nearby.
        //
        public virtual bool IsValid()
        {
            return true;
        }
        //
        // Returns goals priority. This priority can be dynamic. Check
        // `./goals/keep_fed.gd` for an example of dynamic priority.
        //
        public virtual int Priority()
        {
            return 1;
        }

        //
        // Plan's desired state. This is usually referred as desired world
        // state, but it doesn't need to match the raw world state.
        //
        // For example, in your world state you may store "hunger" as a number, but inside your
        // goap you can deal with it as "is_hungry".
        public virtual Dictionary<object, object> GetDesiredState()
        {
            return new Dictionary<object, object>();
        }
    }
}