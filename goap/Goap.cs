namespace CSGoap
{
	// This class is an Autoload accessible globaly.
	// It initialises a GoapActionPlanner with the available
	// actions.
	//
	// In your game, you might want to have different planners
	// for different enemy/npc types, and even change the set
	// of actions in runtime.
	//
	// This example keeps things simple, creating only one planner
	// with pre-defined actions.
	//

	using Godot;
	public partial class Goap : Node
	{
		private GoapActionPlanner actionPlanner =  new GoapActionPlanner();
		private static Goap instance = null;

		public static Goap Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Goap();
				}
				return instance;
			}
		}

		public Goap()
		{
			actionPlanner.SetActions(new GoapAction[] {
				new BuildFirepitAction(),
				new ChopTreeAction(),
				new CollectFromWoodStockAction(),
				new CalmDownAction(),
				new FindCoverAction(),
				new FindFoodAction()
			});
		}

		public GoapActionPlanner GetActionPlanner()
		{
			return actionPlanner;
		}
	}
}
