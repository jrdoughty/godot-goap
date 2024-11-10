//
// Goal contract
//
class GoapGoal extends Node
{

    //
    // This indicates if the goal should be considered or not.
    // Sometimes instead of changing the priority, it is easier to
    // not even consider the goal. i.e. Ignore combat related goals
    // when there are not enemies nearby.
    //
    public bool IsValid()
    {
        return true;
    }
    //
    // Returns goals priority. This priority can be dynamic. Check
    // `./goals/keep_fed.gd` for an example of dynamic priority.
    //
    public in Priority()
    {
        return 1;
    }

    //
    // Plan's desired state. This is usually referred as desired world
    // state, but it doesn't need to match the raw world state.
    //
    // For example, in your world state you may store "hunger" as a number, but inside your
    // goap you can deal with it as "is_hungry".
    public Dictionary GetDesiredState()
    {
        return new Dictionary<object, object>();
    }
}