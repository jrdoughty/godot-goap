namespace CSGoap
{
	using Godot;
	using System;

	public partial class Satyr : CharacterBody2D
	{
		[Export] public NodePath HungryLabelPath;
		[Export] public NodePath BodyPath;
		[Export] public NodePath CalmDownTimerPath;

		private Label _hungryLabel;
		private AnimatedSprite2D _body;
		private Timer _calmDownTimer;

		private bool isAttacking = false;
		private bool isMoving = false;

		public override void _Ready()
		{
			_hungryLabel = GetNode<Label>(HungryLabelPath);
			_body = GetNode<AnimatedSprite2D>(BodyPath);
			_calmDownTimer = GetNode<Timer>(CalmDownTimerPath);
		}

		public override void _Process(double delta)
		{
			_hungryLabel.Visible = (int)WorldState.Instance.GetState("hunger", 0) >= 50;

			if (isAttacking)
			{
				_body.Play("attack");
			}
			else if (isMoving)
			{
				isMoving = false;
			}
			else
			{
				_body.Play("idle");
			}
		}

		public void MoveTo(Vector2 direction, double delta)
		{
			isMoving = true;
			isAttacking = false;
			_body.Play("run");
			if (direction.X > 0)
			{
				TurnRight();
			}
			else
			{
				TurnLeft();
			}

			MoveAndCollide(direction * (float)delta * 100);
		}

		private void TurnRight()
		{
			if (!_body.FlipH)
			{
				return;
			}

			_body.FlipH = false;
		}

		private void TurnLeft()
		{
			if (_body.FlipH)
			{
				return;
			}

			_body.FlipH = true;
		}

		public bool ChopTree(Node2D tree)
		{
			// Assuming the tree has a method called Chop
			bool isFinished = ((dynamic)tree).Chop();
			isAttacking = !isFinished;
			return isFinished;
		}

		public bool CalmDown()
		{
			if (!(bool)WorldState.Instance.GetState("is_frightened"))
			{
				return true;
			}

			if (_calmDownTimer.IsStopped())
			{
				_calmDownTimer.Start();
			}

			return false;
		}

		private void _OnDetectionRadiusBodyEntered(Node body)
		{
			if (body.IsInGroup("troll"))
			{
				WorldState.Instance.SetState("is_frightened", true);
			}
		}

		private void _OnCalmDownTimerTimeout()
		{
			WorldState.Instance.SetState("is_frightened", false);
		}
	}
}
