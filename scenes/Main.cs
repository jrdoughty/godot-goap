namespace CSGoap
{
    using Godot;
    using System;
    using System.Collections.Generic;

    public partial class Main : Node2D
    {
        private ProgressBar hungerField;
        public List<GoapAgent> agents;

        public override void _Ready()
        {
            hungerField = GetNode<ProgressBar>("HUD/VBoxContainer/MarginContainer/HBoxContainer/hunger");
            Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup("satyr");
            agents = new List<GoapAgent>();
            foreach (Satyr node in nodes)
            {
                agents.Add((node as Satyr).agent as GoapAgent);
            }
        }

        private void OnHungerTimerTimeout()
        {
            foreach (var agent in agents)
            {
                agent.SetState("hunger", Convert.ToDouble(agent.GetState("hunger", 0)) + 4);
            }
        }

        private void OnReloadPressed()
        {
            WorldState.Instance.ClearState();
            GetTree().ReloadCurrentScene();
        }

        private void OnPausePressed()
        {
            GetTree().Paused = !GetTree().Paused;
            GetNode<Label>("HUD/VBoxContainer/MarginContainer/HBoxContainer/pause").Text = (
                GetTree().Paused ? "Resume" : "Pause"
            );
        }

        private void OnConsolePressed()
        {
            var console = GetTree().GetNodesInGroup("console")[0] as Control;
            console.Visible = !console.Visible;
            GetNode<Button>("HUD/VBoxContainer/MarginContainer/HBoxContainer/console").Text = (
                console.Visible ? "Hide Console" : "Show Console"
            );
        }
    }
}