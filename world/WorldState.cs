namespace CSGoap
{
    using Godot;
    using System.Collections.Generic;

    public partial class WorldState : Node
    {
        public Dictionary<object, object> state = new Dictionary<object, object>();
        private static WorldState instance = null;

        public static WorldState Instance
        {
            get
            {
                instance ??= new WorldState();
                return instance;
            }
        }

        public override void _Ready()
        {
            instance = this;
        }

        public object GetState(object stateName, object defaultValue = null)
        {
            if (state.TryGetValue(stateName, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        public void SetState(object stateName, object value)
        {
            state[stateName] = value;
        }

        public void ClearState()
        {
            state.Clear();
        }

        public Godot.Collections.Array<Godot.Node> GetElements(string groupName)
        {
            return GetTree().GetNodesInGroup(groupName);
        }

        public Node2D GetClosestElement(string groupName, Node2D reference)
        {
            var elements = GetElements(groupName);
            Node2D closestElement = null;
            float closestDistance = float.MaxValue;

            foreach (Node2D element in elements)
            {
                float distance = reference.Position.DistanceTo(element.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestElement = element;
                }
            }

            return closestElement;
        }

        public void ConsoleMessage(string message)
        {
            var console = GetTree().GetNodesInGroup("console")[0] as Node;
            if (console != null)
            {
                var method = console.GetType().GetMethod("AddMessage");
                if (method != null)
                {
                    method.Invoke(console, new object[] { message });
                }
            }
        }
    }
}
