namespace CSGoap
{
    using Godot;
    using System;

    public partial class Main : Node2D
    {
        private ProgressBar hungerField;
        public GoapAgent agent;

        public override void _Ready()
        {
            hungerField = GetNode<ProgressBar>("HUD/VBoxContainer/MarginContainer/HBoxContainer/hunger");
            agent = GetNode<Satyr>("satyr").agent;
        }

        private void OnHungerTimerTimeout()
        {
            hungerField.Value = Convert.ToInt32(agent.GetState("hunger", 0));
            if (hungerField.Value < 100)
            {
                hungerField.Value += 2;
            }

            agent.SetState("hunger", hungerField.Value);
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