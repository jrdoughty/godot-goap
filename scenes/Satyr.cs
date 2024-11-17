namespace CSGoap
{
	using Godot;
	using System;
	using System.Collections.Generic;

	public partial class Satyr : CharacterBody2D
	{
		[Export] public NodePath HungryLabelPath;
		[Export] public NodePath BodyPath;
		[Export] public NodePath CalmDownTimerPath;
		[Export] public NodePath AgentPath;

		private Label _hungryLabel;
		private AnimatedSprite2D _body;
		private Timer _calmDownTimer;

		private bool isAttacking = false;
		private bool isMoving = false;
		public GoapAgent agent;
		public List<GoapGoal> goals;
		public List<GoapAction> actions;

		public override void _Ready()
		{
			_hungryLabel = GetNode<Label>(HungryLabelPath);
			_body = GetNode<AnimatedSprite2D>(BodyPath);
			_calmDownTimer = GetNode<Timer>(CalmDownTimerPath);
			agent = GetNode<GoapAgent>(AgentPath) as GoapAgent;
			
		}

		public override void _Process(double delta)
		{
			_hungryLabel.Visible = Convert.ToInt32(agent.GetState("hunger", 0)) >= 50;

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

		private void OnDetectionRadiusBodyEntered(Node body)
		{
			if (body.IsInGroup("troll"))
			{
				WorldState.Instance.SetState("is_frightened", true);
			}
		}

		private void OnCalmDownTimerTimeout()
		{
			WorldState.Instance.SetState("is_frightened", false);
		}
	}
}
