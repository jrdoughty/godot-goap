namespace CSGoap
{
    using Godot;
    using System;

    public partial class Firepit : Node2D
    {
        [Export] private NodePath labelPath;
        [Export] private NodePath timerPath;

        private Label _label;
        private Timer _timer;

        public override void _Ready()
        {
            _label = GetNode<Label>(labelPath);
            _timer = GetNode<Timer>(timerPath);
        }

        public override void _Process(double delta)
        {
            _label.Text = Math.Ceiling(_timer.TimeLeft).ToString();
        }

        private void OnTimerTimeout()
        {
            QueueFree();
        }
    }
}