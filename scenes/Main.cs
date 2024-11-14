namespace CSGoap
{
    using Godot;
    using System;

    public partial class Main : Node2D
    {
        private ProgressBar _hungerField;

        public override void _Ready()
        {
            _hungerField = GetNode<ProgressBar>("HUD/VBoxContainer/MarginContainer/HBoxContainer/hunger");
        }

        private void OnHungerTimerTimeout()
        {
            _hungerField.Value = Convert.ToInt32(WorldState.Instance.GetState("hunger", 0));
            if (_hungerField.Value < 100)
            {
                _hungerField.Value += 25;
            }

            WorldState.Instance.SetState("hunger", _hungerField.Value);
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
            GetNode<Label>("HUD/VBoxContainer/MarginContainer/HBoxContainer/console").Text = (
                console.Visible ? "Hide Console" : "Show Console"
            );
        }
    }
}