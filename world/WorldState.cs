namespace CSGoap
{
   
    using Godot;
    using System.Collections.Generic;

    class WorldState : Node
    {
        Dictionary<object, object> state = new Dictionary<object, object>();
        private static WorldState instance = null;

        public static WorldState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorldState();
                }
                return instance;
            }
        }

        public object GetState(object state_name, object default_value = null)
        {
            if (state.TryGetValue(state_name, out var value))
            {
                return value;
            }
            return default_value;
        }

        public void SetState(object state_name, object value)
        {
            if (state.TryGetValue(stateName, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        public void ClearState()
        {
            state = new Dictionary<object, object>();
        }

        public Node[] GetElements(string group_name)
        {
            return GetTree().GetNodesInGroup(group_name);
        }

        public Node GetClosestElement(string group_name, Node2D reference)
        {
            List<Node2D> elements = new List<Node2D>();
            Node[] elnts = GetElements(group_name);
            foreach (Node el in elnts)
            {
                if (!(el is Node2D))
                {
                    continue;
                }
                elements.Add(el as Node2D);
            }
            Node2D closestElement = null;
            float closest_distance = float.MaxValue;

            foreach (Node2D element in elements)
            {
                var distance = reference.Position.DistanceTo(element.Position);
                if (distance < closest_distance)
                {
                    closest_distance = distance;
                    closestElement = element;
                }
            }

            return closestElement;
        }

        public void ConsoleMessage(object message)
        {
            TextEdit console = GetTree().GetNodesInGroup("console")[0] as TextEdit;
            if (console != null)
            {
                console.Text += "\n" + message.ToString();
                console.CaretLine = console.GetLineCount();
            }
            else
            {
                GD.Print("no on-screen debug console found");
            }
        }
    }
}