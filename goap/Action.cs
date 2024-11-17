namespace CSGoap
{
   //
    // Action Contract
    //
    using System.Collections.Generic;
    using Godot;

    public partial class GoapAction : Node
    {

        public virtual string GetClazz()
        {
            return "GoapAction";
        }

        //
        // This indicates if the action should be considered or not.
        //
        // Currently I'm using this method only during planning, but it could
        // also be used during execution to abort the plan in case the world state
        // does not allow this action anymore.
        //
        public virtual bool IsValid(GoapAgent agent)
        {
            return true;
        }

        //
        // Action Cost. This is a functiontion so it handles situational costs, when the world
        // state is considered when calculating the cost.
        //
        // Check "./actions/choptreeaction.cs" for a situational cost example.
        //
        public virtual int GetCost(Dictionary<object, object> blackboard)
        {
            return 1000;
        }
        //
        // Action requirements.
        // Example:
        // {
        //   "has_wood": true
        // }
        //
        public virtual Dictionary<object, object> GetPreconditions()
        {
            return new Dictionary<object, object>();
        }

        //
        // What conditions this action satisfies
        //
        // Example:
        // {
        //   "has_wood": true
        // }
        public virtual Dictionary<object, object> GetEffects()
        {
            return new Dictionary<object, object>();
        }

        //
        // Action implementation called on every loop.
        // "actor" is the NPC using the AI
        // "delta" is the time in seconds since last loop.
        //
        // Returns true when the task is complete.
        //
        // Check "./actions/chop_tree.gd" for example.
        //
        // I decided to have actions owning their logic, but this
        // is up to you. You could have another script to handle this
        // or even let your NPC decide how to handle the action. In other words,
        // your NPC could just receive the action name and decide what to do.
        //
        public virtual bool Perform(Node2D actor, double _delta)
        {
            return false;
        }
    }
}