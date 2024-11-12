namespace CSGoap
{
    using Godot;
    using System;

    public class Troll : CharacterBody2D
    {
        private Vector2 _target;
        [Export] private NodePath bodyPath;
        [Export] private NodePath restTimerPath;
        [Export] private NodePath rayCast2DPath;

        private AnimatedSprite2D _body;
        private Timer _restTimer;
        private RayCast2D _rayCast2D;

        public override void _Ready()
        {
            _body = GetNode<AnimatedSprite2D>(bodyPath);
            _restTimer = GetNode<Timer>(restTimerPath);
            _rayCast2D = GetNode<RayCast2D>(rayCast2DPath);

            PickRandomPosition();
            _body.Play("run");
        }

        public override void _Process(double delta)
        {
            if (Position.DistanceTo(_target) > 1)
            {
                Vector2 direction = Position.DirectionTo(_target);
                if (direction.x > 0)
                {
                    TurnRight();
                }
                else
                {
                    TurnLeft();
                }

                MoveAndCollide(direction * delta * 100);
            }
            else
            {
                _body.Play("idle");
                _restTimer.Start();
                SetProcess(false);
            }
        }

        private void PickRandomPosition()
        {
            Random random = new Random();
            _target = new Vector2(random.Next(5, 450), random.Next(5, 250));
        }

        private void OnRestTimerTimeout()
        {
            PickRandomPosition();
            _body.Play("run");
            SetProcess(true);
        }

        private void TurnRight()
        {
            if (!_body.FlipH)
            {
                return;
            }

            _body.FlipH = false;
            _rayCast2D.TargetPosition *= -1;
        }

        private void TurnLeft()
        {
            if (_body.FlipH)
            {
                return;
            }

            _body.FlipH = true;
            _rayCast2D.TargetPosition *= -1;
        }
    }
}